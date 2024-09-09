using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Repository.DAL;
using Service.ViewModels.Basket;

namespace DunkerFinal.Controllers
{
    public class BasketController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public BasketController(AppDbContext appDbContext, UserManager<AppUser> userManager)
        {
            _context = appDbContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            AppUser existUser = new();

            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            else
            {
                existUser = await _userManager.FindByNameAsync(User.Identity.Name);
            }

            Basket basket = await _context.Baskets?
                .Include(m => m.BasketProducts)
                .ThenInclude(m => m.Product)
                .ThenInclude(m => m.ProductImages)
                .FirstOrDefaultAsync(m => m.AppUserId == existUser.Id);

            List<BasketListVM> basketListVMs = new();

            if (basket != null)
            {
                foreach (var item in basket.BasketProducts)
                {
                    basketListVMs.Add(new BasketListVM()
                    {
                        Id = item.Id,
                        Image = item.Product.ProductImages.FirstOrDefault(m => m.IsMain)?.Image,
                        Name = item.Product.Name,
                        Price = item.Product.Price,
                        Quantity = item.Quantity,
                        TotalPrice = item.Product.Price * item.Quantity
                    });
                }
            }
            ViewBag.BasketProduct = basketListVMs;

            return View(basketListVMs);
        }


        [HttpPost]
        public async Task<IActionResult> Add(int? productId)
        {
            if (productId == null)
                return BadRequest(new { success = false, message = "Product ID is required." });

            if (!User.Identity.IsAuthenticated)
            {
                // Return a JSON response indicating that the user is not authenticated
                return Json(new { success = false, message = "User is not authenticated.", redirectUrl = Url.Action("Login", "Account") });
            }

            AppUser existUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (existUser == null)
                return NotFound();

            Product existProduct = await _context.Products.FirstOrDefaultAsync(m => m.Id == productId);
            if (existProduct == null)
                return NotFound();

            BasketProduct basketProduct = await _context.BasketProducts
                .FirstOrDefaultAsync(m => m.ProductId == productId && m.Basket.AppUserId == existUser.Id);
            Basket basket = await _context.Baskets
                .Include(m => m.BasketProducts)
                .FirstOrDefaultAsync(m => m.AppUserId == existUser.Id);

            if (basketProduct != null)
            {
                basketProduct.Quantity++;
                await _context.SaveChangesAsync();

                // Send a response that only updates the product count
                return Json(new { success = true, message = "Product quantity updated.", uniqueProductCount = basket.BasketProducts.Count, isUpdate = true });
            }
            else
            {
                if (basket != null)
                {
                    basket.BasketProducts.Add(new BasketProduct()
                    {
                        ProductId = (int)productId,
                        Quantity = 1
                    });
                }
                else
                {
                    Basket newBasket = new()
                    {
                        AppUserId = existUser.Id,
                    };

                    await _context.Baskets.AddAsync(newBasket);
                    await _context.SaveChangesAsync();

                    BasketProduct newBasketProduct = new()
                    {
                        BasketId = newBasket.Id,
                        ProductId = (int)productId,
                        Quantity = 1
                    };

                    await _context.BasketProducts.AddAsync(newBasketProduct);
                }

                await _context.SaveChangesAsync();

                // This part renders the partial view for the updated basket
                BasketProduct addedProduct = await _context.BasketProducts
                    .Include(m => m.Product).ThenInclude(m => m.ProductImages)
                    .Include(m => m.Product).ThenInclude(m => m.Category)
                    .FirstOrDefaultAsync(m => m.ProductId == productId && m.Basket.AppUserId == existUser.Id);

                string renderedView = await RenderViewAsync("_SidebarBasket", addedProduct);

                return Json(new { success = true, message = "Product added to basket.", uniqueProductCount = basket.BasketProducts.Count, partialView = renderedView, isUpdate = false });
            }
        }


        private async Task<string> RenderViewAsync(string viewName, object model, bool partial = false)
        {
            var viewEngine = HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
            var tempDataProvider = HttpContext.RequestServices.GetService(typeof(ITempDataProvider)) as ITempDataProvider;
            var actionContext = new ActionContext(HttpContext, HttpContext.GetRouteData(), ControllerContext.ActionDescriptor, ModelState);

            using (var sw = new StringWriter())
            {
                var viewResult = viewEngine.FindView(actionContext, viewName, !partial);
                if (viewResult.View == null)
                {
                    throw new ArgumentNullException($"{viewName} does not match any available view");
                }

                var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), ModelState)
                {
                    Model = model
                };
                var viewContext = new ViewContext(actionContext, viewResult.View, viewDictionary, new TempDataDictionary(HttpContext, tempDataProvider), sw, new HtmlHelperOptions());

                await viewResult.View.RenderAsync(viewContext);
                return sw.ToString();
            }
        }



        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return Json(new { success = false, message = "Product ID is missing." });

            if (!User.Identity.IsAuthenticated)
                return Json(new { success = false, message = "User not authenticated." });

            var existUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (existUser == null)
                return Json(new { success = false, message = "User not found." });

            var basketProduct = await _context.BasketProducts
                .Include(m => m.Product)
                .FirstOrDefaultAsync(m => m.Id == id && m.Basket.AppUserId == existUser.Id);

            if (basketProduct == null)
                return Json(new { success = false, message = "Product not found in basket." });

            _context.BasketProducts.Remove(basketProduct);
            await _context.SaveChangesAsync();

            var totalPrice = await _context.BasketProducts
                .Where(m => m.Basket.AppUserId == existUser.Id)
                .SumAsync(m => m.Product.Price * m.Quantity);

            var totalQuantity = await _context.BasketProducts
                .Where(m => m.Basket.AppUserId == existUser.Id)
                .SumAsync(m => m.Quantity);

            int basketCount = await _context.BasketProducts
                .Where(m => m.Basket.AppUserId == existUser.Id)
                .CountAsync();

            return Json(new
            {
                success = true,
                totalPrice,
                totalQuantity,
                basketCount,
                isEmpty = totalQuantity == 0
            });
        }

        public async Task<IActionResult> OrderConfirmation()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            AppUser existUser = await _userManager.FindByNameAsync(User.Identity.Name);

            if (existUser == null)
                return NotFound();

            Basket basket = await _context.Baskets
                .Include(b => b.BasketProducts)
                .FirstOrDefaultAsync(b => b.AppUserId == existUser.Id);

            if (basket != null && basket.BasketProducts.Any())
            {
                _context.BasketProducts.RemoveRange(basket.BasketProducts);
                await _context.SaveChangesAsync();
            }

            // You can return the confirmation view or redirect as needed
            return View();
        }


        public class IncreaseRequest
        {
            public int Id { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> Increase([FromBody] IncreaseRequest request)
        {
            if (request.Id <= 0)
                return BadRequest("Invalid product ID.");

            var userName = User.Identity?.Name;
            if (string.IsNullOrEmpty(userName))
                return Unauthorized("User is not authenticated.");

            AppUser existUser = await _userManager.FindByNameAsync(userName);
            if (existUser == null)
                return Unauthorized("User not found.");

            BasketProduct basketProduct = await _context.BasketProducts
                .Include(m => m.Product)
                .FirstOrDefaultAsync(m => m.Id == request.Id && m.Basket.AppUserId == existUser.Id);

            if (basketProduct == null)
                return NotFound("Product not found in basket.");

            basketProduct.Quantity++;
            await _context.SaveChangesAsync();

            decimal totalPrice = basketProduct.Product.Price * basketProduct.Quantity;
            decimal total = await _context.BasketProducts
                .Where(m => m.Basket.AppUserId == existUser.Id)
                .SumAsync(m => m.Quantity);

            return Ok(new { totalPrice, total });
        }
        [HttpPost]
        public async Task<IActionResult> Decrease([FromBody] ProductRequest request)
        {
            // Validate the product ID
            if (request == null || request.Id <= 0)
            {
                return BadRequest(new { success = false, message = "Invalid product ID." });
            }

            // Ensure the user is authenticated
            var userName = User.Identity?.Name;
            if (string.IsNullOrEmpty(userName))
            {
                return Unauthorized(new { success = false, message = "User is not authenticated." });
            }

            // Find the user
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return Unauthorized(new { success = false, message = "User not found." });
            }

            // Find the basket product
            var basketProduct = await _context.BasketProducts
                .Include(bp => bp.Product)
                .FirstOrDefaultAsync(bp => bp.Id == request.Id && bp.Basket.AppUserId == user.Id);

            if (basketProduct == null)
            {
                return NotFound(new { success = false, message = "Product not found in basket." });
            }

            // Decrease quantity or remove product
            if (basketProduct.Quantity > 1)
            {
                basketProduct.Quantity--;
            }
            else
            {
                _context.BasketProducts.Remove(basketProduct);
            }

            await _context.SaveChangesAsync();

            // Calculate the updated total price and quantity
            var totalPrice = await _context.BasketProducts
                .Where(bp => bp.Basket.AppUserId == user.Id)
                .SumAsync(bp => bp.Product.Price * bp.Quantity);

            var totalQuantity = await _context.BasketProducts
                .Where(bp => bp.Basket.AppUserId == user.Id)
                .SumAsync(bp => bp.Quantity);

            int basketCount = await _context.BasketProducts
                .Where(bp => bp.Basket.AppUserId == user.Id)
                .CountAsync();

            return Ok(new { success = true, totalPrice, totalQuantity, basketCount });
        }

        public class ProductRequest
        {
            public int Id { get; set; }
        }


        public async Task<IActionResult> GetCart()
        {
            AppUser existUser = new();

            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            else
            {
                existUser = await _userManager.FindByNameAsync(User.Identity.Name);
            }

            Basket basket = await _context.Baskets?
                .Include(m => m.BasketProducts)
                .ThenInclude(m => m.Product)
                .ThenInclude(m => m.ProductImages)
                .FirstOrDefaultAsync(m => m.AppUserId == existUser.Id);


            if (basket.BasketProducts.Count == 0)
            {
                return Content("");
            }

            return View();
        }


    }
}
