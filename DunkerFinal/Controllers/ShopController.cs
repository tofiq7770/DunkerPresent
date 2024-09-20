using Domain.Entities;
using DunkerFinal.Areas.Admin.Class;
using DunkerFinal.ViewModels.Shop;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.DAL;
using Service.Services.Interfaces;
using System.Security.Claims;

namespace DunkerFinal.Controllers
{
    public class ShopController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IBrandService _brandService;
        private readonly ISliderService _sliderService;
        private readonly ITagService _tagService;
        private readonly IColorService _colorService;
        private readonly IProductTagService _tagProductService;
        private readonly IProductColorService _colorProductService;

        public ShopController(IProductService productService,
                              ICategoryService categoryService,
                              AppDbContext context,
                              UserManager<AppUser> userManager,
                              SignInManager<AppUser> signInManager,
                              IHttpContextAccessor httpContextAccessor,
                              ISliderService sliderService,
                              IBrandService brandService,
                              IProductColorService colorProductService,
                              IColorService colorService,
                              ITagService tagService,
                              IProductTagService tagProductTagService
                              )
        {
            _context = context;
            _productService = productService;
            _categoryService = categoryService;
            _colorProductService = colorProductService;
            _userManager = userManager;
            _signInManager = signInManager;
            _sliderService = sliderService;
            _tagService = tagService;
            _colorService = colorService;
            _tagProductService = tagProductTagService;
            _brandService = brandService;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 9)
        {
            var userId = _userManager.GetUserId(User);

            var products = await _productService.GetAllAsync();
            var paginatedProducts = PagedList<Product>.Create(products, pageNumber, pageSize);

            ShopVM model = new ShopVM
            {
                Products = paginatedProducts,
                Categories = await _categoryService.GetAllAsync(),
                Brands = await _brandService.GetAllAsync(),
                Colors = await _colorService.GetAllAsync(),
                Wishlists = await _context.Wishlists.ToListAsync(),
                ProductColors = await _context.ProductColors.ToListAsync(),
                ProductTags = await _context.ProductTags.ToListAsync(),
                Tags = await _tagService.GetAllAsync(),
                WishlistProducts = await _context.WishlistProducts.Where(wp => wp.Wishlist.AppUserId == userId).ToListAsync(),
                Baskets = await _context.BasketProducts.Where(bp => bp.Basket.AppUserId == userId).ToListAsync(),

                PageNumber = pageNumber,
                TotalPages = paginatedProducts.TotalPages,
            };

            return View(model);
        }




        public async Task<IActionResult> ProductDetail(int? id)
        {
            if (id == null)
            {

                return BadRequest("Product ID is required.");
            }

            var userId = User.Identity.IsAuthenticated ? _userManager.GetUserId(User) : null;
            var product = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductTags).ThenInclude(pt => pt.Tag)
                .Include(p => p.ProductColors).ThenInclude(pc => pc.Color)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Reviews).ThenInclude(r => r.AppUser)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {

                throw new Exception("Product not found.");
                return NotFound("Product not found.");
            }

            var viewModel = new ShopVM
            {
                Product = product,
                Reviews = await _context.Reviews.Include(r => r.AppUser).ToListAsync(),
                UserName = User.Identity.IsAuthenticated ? User.Identity.Name : string.Empty,
                UserEmail = User.Identity.IsAuthenticated ? User.FindFirst(ClaimTypes.Email)?.Value : string.Empty,
                Categories = await _categoryService.GetAllAsync(),
                Brands = await _brandService.GetAllAsync(),
                Colors = await _colorService.GetAllAsync(),
                WishlistProducts = User.Identity.IsAuthenticated
                    ? await _context.WishlistProducts.Where(bp => bp.Wishlist.AppUserId == userId).ToListAsync()
                    : new List<WishlistProduct>(),
                Baskets = User.Identity.IsAuthenticated
                    ? await _context.BasketProducts.Where(bp => bp.Basket.AppUserId == userId).ToListAsync()
                    : new List<BasketProduct>(),
                Wishlists = await _context.Wishlists.ToListAsync(),
                ProductColors = await _context.ProductColors.ToListAsync(),
                ProductTags = await _context.ProductTags.ToListAsync()
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Sorting(string sort, int pageNumber = 1, int pageSize = 9)
        {
            if (_productService == null)
            {
                throw new InvalidOperationException("Product service is not initialized.");
            }

            IEnumerable<Product> products = await _productService.GetAllAsync();
            if (products == null)
            {
                throw new InvalidOperationException("Products could not be retrieved.");
            }

            // Sorting logic
            switch (sort)
            {
                case "SORT BY RATING":
                    products = products.OrderByDescending(m => m.Rating);
                    break;
                case "SORT BY LATEST":
                    products = products.OrderByDescending(m => m.CreatedTime);
                    break;
                case "SORT BY PRICE HIGH TO LOW":
                    products = products.OrderByDescending(m => m.Price);
                    break;
                case "SORT BY PRICE LOW TO HIGH":
                    products = products.OrderBy(m => m.Price);
                    break;
                default:
                    products = products.OrderBy(m => m.Name);
                    break;
            }

            // Convert to list before applying pagination
            var productList = products.ToList();

            // Apply pagination
            var paginatedProducts = PagedList<Product>.Create(productList, pageNumber, pageSize);

            ShopVM model = new()
            {
                Products = paginatedProducts,
                Brands = await _brandService.GetAllAsync(),
                Categories = await _categoryService.GetAllAsync(),
                Colors = await _colorService.GetAllAsync(),
                WishlistProducts = await _context.WishlistProducts.ToListAsync(),
                Wishlists = await _context.Wishlists.ToListAsync(),
                ProductColors = await _context.ProductColors.ToListAsync(),
                ProductTags = await _context.ProductTags.ToListAsync(),
                PageNumber = pageNumber,
                TotalPages = paginatedProducts.TotalPages
            };

            return View("Index", model);
        }


        public async Task<IActionResult> Search(string searchText, int pageNumber = 1, int pageSize = 9)
        {
            if (_productService == null)
            {
                throw new InvalidOperationException("Product service is not initialized.");
            }

            List<Product> products;

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                products = await _productService.GetAllAsync();
                products = products.Where(p => p.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            else
            {
                products = await _productService.GetAllAsync();
            }

            // Apply pagination
            var paginatedProducts = PagedList<Product>.Create(products, pageNumber, pageSize);

            ShopVM model = new()
            {
                Products = paginatedProducts,
                Brands = await _brandService.GetAllAsync(),
                Categories = await _categoryService.GetAllAsync(),
                Colors = await _colorService.GetAllAsync(),
                WishlistProducts = await _context.WishlistProducts.ToListAsync(),
                Wishlists = await _context.Wishlists.ToListAsync(),
                ProductColors = await _context.ProductColors.ToListAsync(),
                ProductTags = await _context.ProductTags.ToListAsync(),
                PageNumber = pageNumber,
                TotalPages = paginatedProducts.TotalPages
            };

            return View("Index", model);
        }


        //public async Task<IActionResult> Sorting(string sort)
        //{
        //    if (_productService == null)
        //    {
        //        throw new InvalidOperationException("Product service is not initialized.");
        //    }

        //    IEnumerable<Product> products = await _productService.GetAllAsync();
        //    if (products == null)
        //    {
        //        throw new InvalidOperationException("Products could not be retrieved.");
        //    }

        //    switch (sort)
        //    {
        //        case "SORT BY RATING":
        //            products = products.OrderByDescending(m => m.Rating);
        //            break;
        //        case "SORT BY LATEST":
        //            products = products.OrderByDescending(m => m.CreatedTime);
        //            break;
        //        case "SORT BY PRICE HIGH TO LOW":
        //            products = products.OrderByDescending(m => m.Price);
        //            break;
        //        case "SORT BY PRICE LOW TO HIGH":
        //            products = products.OrderBy(m => m.Price);
        //            break;
        //        default:
        //            products = products.OrderBy(m => m.Name);
        //            break;
        //    }

        //    ShopVM model = new()
        //    {
        //        Products = products,
        //        Brands = await _brandService.GetAllAsync(),
        //        Categories = await _categoryService.GetAllAsync(),
        //        Colors = await _colorService.GetAllAsync(),
        //        WishlistProducts = await _context.WishlistProducts.ToListAsync(),
        //        Wishlists = await _context.Wishlists.ToListAsync(),
        //        ProductColors = await _context.ProductColors.ToListAsync(),
        //        ProductTags = await _context.ProductTags.ToListAsync(),
        //    };
        //    if (model == null)
        //    {
        //        throw new InvalidOperationException("Model could not be created.");
        //    }

        //    return View("Index", model);
        //}

        //public async Task<IActionResult> Search(string searchText)
        //{
        //    if (_productService == null)
        //    {
        //        throw new InvalidOperationException("Product service is not initialized.");
        //    }

        //    IEnumerable<Product> products;

        //    if (!string.IsNullOrWhiteSpace(searchText))
        //    {
        //        products = await _productService.GetAllAsync();
        //        products = products.Where(p => p.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase));
        //    }
        //    else
        //    {
        //        products = await _productService.GetAllAsync();
        //    }

        //    ShopVM model = new()
        //    {
        //        Products = products,
        //        Brands = await _brandService.GetAllAsync(),
        //        Categories = await _categoryService.GetAllAsync(),
        //        Colors = await _colorService.GetAllAsync(),
        //        WishlistProducts = await _context.WishlistProducts.ToListAsync(),
        //        Wishlists = await _context.Wishlists.ToListAsync(),
        //        ProductColors = await _context.ProductColors.ToListAsync(),
        //        ProductTags = await _context.ProductTags.ToListAsync(),
        //    };

        //    return View("Index", model);
        //}


        public async Task<IActionResult> Filter(IEnumerable<int> categories, IEnumerable<int> brands, IEnumerable<int> colors, int minPrice, int maxPrice, int pageNumber = 1, int pageSize = 9)
        {
            var products = await _productService.GetAllAsync();

            if (categories != null && categories.Any())
            {
                products = products.Where(p => categories.Contains(p.CategoryId)).ToList();
            }

            if (brands != null && brands.Any())
            {
                products = products.Where(p => brands.Contains(p.BrandId)).ToList();
            }

            if (colors != null && colors.Any())
            {
                products = products.Where(p => p.ProductColors != null && p.ProductColors.Any(pc => colors.Contains(pc.ColorId))).ToList();
            }

            products = products.Where(p => p.Price >= minPrice && p.Price <= maxPrice).ToList();

            var paginatedProducts = PagedList<Product>.Create(products, pageNumber, pageSize);

            ShopVM model = new()
            {
                Products = paginatedProducts,
                Brands = await _brandService.GetAllAsync(),
                Categories = await _categoryService.GetAllAsync(),
                Colors = await _colorService.GetAllAsync(),
                WishlistProducts = await _context.WishlistProducts.ToListAsync(),
                Wishlists = await _context.Wishlists.ToListAsync(),
                ProductColors = await _context.ProductColors.ToListAsync(),
                ProductTags = await _context.ProductTags.ToListAsync(),
                PageNumber = pageNumber,
                TotalPages = paginatedProducts.TotalPages
            };

            return View("Index", model);
        }


        //public async Task<IActionResult> Filter(IEnumerable<int> categories, IEnumerable<int> brands, IEnumerable<int> colors, int minPrice, int maxPrice)
        //{
        //    var products = await _productService.GetAllAsync();

        //    if (categories != null && categories.Any())
        //    {
        //        products = products.Where(p => categories.Contains(p.CategoryId)).ToList();
        //    }

        //    if (brands != null && brands.Any())
        //    {
        //        products = products.Where(p => brands.Contains(p.BrandId)).ToList();
        //    }

        //    if (colors != null && colors.Any())
        //    {
        //        products = products.Where(p => p.ProductColors != null && p.ProductColors.Any(pc => colors.Contains(pc.ColorId))).ToList();
        //    }

        //    products = products.Where(p => p.Price >= minPrice && p.Price <= maxPrice).ToList();


        //    ShopVM model = new()
        //    {
        //        Products = products,
        //        Brands = await _brandService.GetAllAsync(),
        //        Categories = await _categoryService.GetAllAsync(),
        //        Colors = await _colorService.GetAllAsync(),
        //        WishlistProducts = await _context.WishlistProducts.ToListAsync(),
        //        Wishlists = await _context.Wishlists.ToListAsync(),
        //        ProductColors = await _context.ProductColors.ToListAsync(),
        //        ProductTags = await _context.ProductTags.ToListAsync(),
        //    };

        //    return View("Index", model);
        //}

        [HttpPost]
        public async Task<IActionResult> AddComment(int productId, string message, int rating)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { success = false, message = "You need to be logged in to add a comment." });
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "User ID could not be found." });
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return Json(new { success = false, message = "User not found." });
            }

            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return Json(new { success = false, message = "Product not found." });
            }

            var review = new Review
            {
                ProductId = productId,
                Message = message,
                CreatedTime = DateTime.Now,
                AppUser = user,
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return Json(new { success = true, reviewId = review.Id }); // Include reviewId in the response
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { success = false, message = "You need to be logged in to delete a comment." });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var comment = await _context.Reviews
                                        .Include(r => r.AppUser)
                                        .FirstOrDefaultAsync(r => r.Id == commentId);

            if (comment == null)
            {
                return Json(new { success = false, message = "Comment not found." });
            }

            if (comment.AppUser.Id != userId)
            {
                return Json(new { success = false, message = "You are not authorized to delete this comment." });
            }

            _context.Reviews.Remove(comment);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Comment deleted successfully." });
        }

    }
}
