using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.DAL;
using Service.ViewModels.Wishlist;

namespace DunkerFinal.Controllers
{

    public class WishlistController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public WishlistController(AppDbContext appDbContext, UserManager<AppUser> userManager)
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

            Wishlist wishlist = await _context.Wishlists?
                .Include(m => m.WishlistProducts)
                .ThenInclude(m => m.Product)
                .ThenInclude(m => m.ProductImages)
                .FirstOrDefaultAsync(m => m.AppUserId == existUser.Id);

            List<WishlistListVM> wishlistListVMs = new();

            if (wishlist != null)
            {
                foreach (var item in wishlist.WishlistProducts)
                {
                    wishlistListVMs.Add(new WishlistListVM()
                    {
                        Id = item.Id,
                        Image = item.Product.ProductImages.FirstOrDefault(m => m.IsMain)?.Image,
                        Name = item.Product.Name,
                        Price = item.Product.Price,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                    });
                }
            }
            ViewBag.WishlistProduct = wishlistListVMs;

            return View(wishlistListVMs);
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ProductRequest request)
        {
            int? productId = request.ProductId;

            if (productId == null)
            {
                return BadRequest(new { success = false, message = "Product ID is required." });
            }

            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { success = false, message = "User is not authenticated.", redirectUrl = Url.Action("Login", "Account") });
            }

            var existUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (existUser == null)
            {
                return Json(new { success = false, message = "User not found." });
            }

            var existProduct = await _context.Products.FindAsync(productId);
            if (existProduct == null)
            {
                return Json(new { success = false, message = "Product not found." });
            }

            var wishlist = await _context.Wishlists
                .Include(w => w.WishlistProducts)
                .FirstOrDefaultAsync(w => w.AppUserId == existUser.Id);

            if (wishlist == null)
            {
                wishlist = new Wishlist { AppUserId = existUser.Id };
                await _context.Wishlists.AddAsync(wishlist);
                await _context.SaveChangesAsync();
            }

            var wishlistProduct = await _context.WishlistProducts
                .FirstOrDefaultAsync(wp => wp.ProductId == productId && wp.WishlistId == wishlist.Id);

            bool alreadyInWishlist = wishlistProduct != null;

            if (alreadyInWishlist)
            {
                wishlistProduct.Quantity++;
            }
            else
            {
                var newWishlistProduct = new WishlistProduct
                {
                    WishlistId = wishlist.Id,
                    ProductId = (int)productId,
                    Quantity = 1
                };
                await _context.WishlistProducts.AddAsync(newWishlistProduct);
            }

            await _context.SaveChangesAsync();

            int uniqueProductCount = await _context.WishlistProducts
                .Where(wp => wp.Wishlist.AppUserId == existUser.Id)
                .CountAsync();

            return Json(new { success = true, message = alreadyInWishlist ? "Product already in wishlist." : "Product added to wishlist.", uniqueProductCount, alreadyInWishlist });
        }


        public class ProductRequest
        {
            public int ProductId { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] DeleteRequest request)
        {
            if (request.Id == null)
                return Json(new { success = false, message = "Product ID is missing." });

            if (!User.Identity.IsAuthenticated)
                return Json(new { success = false, message = "User not authenticated." });

            var existUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (existUser == null)
                return Json(new { success = false, message = "User not found." });

            var basketProduct = await _context.WishlistProducts
                .Include(m => m.Product)
                .FirstOrDefaultAsync(m => m.Id == request.Id && m.Wishlist.AppUserId == existUser.Id);

            if (basketProduct == null)
                return Json(new { success = false, message = "Product not found in wishlist." });

            _context.WishlistProducts.Remove(basketProduct);
            await _context.SaveChangesAsync();

            var totalPrice = await _context.WishlistProducts
                .Where(m => m.Wishlist.AppUserId == existUser.Id)
                .SumAsync(m => m.Product.Price * m.Quantity);

            var totalQuantity = await _context.WishlistProducts
                .Where(m => m.Wishlist.AppUserId == existUser.Id)
                .SumAsync(m => m.Quantity);

            int basketCount = await _context.WishlistProducts
                .Where(m => m.Wishlist.AppUserId == existUser.Id)
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

        public class DeleteRequest
        {
            public int Id { get; set; }
        }


    }
}
