using Domain.Entities;
using DunkerFinal.ViewModels.Home;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.DAL;
using Service.Services.Interfaces;

namespace DunkerFinal.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IProductService _productService;
        private readonly IBrandService _brandService;
        private readonly IInfoBannerService _infoService;
        private readonly IBannerService _bannerService;
        private readonly ISliderService _sliderService;
        private readonly ITagService _tagService;
        private readonly IColorService _colorService;
        private readonly IProductTagService _tagProductService;
        private readonly ICategoryService _categoryService;

        public HomeController(IProductService productService,
                              ISliderService sliderService,
                              IInfoBannerService infoService,
                              UserManager<AppUser> userManager,
                              IBrandService brandService,
                              IBannerService bannerService,
                              IColorService colorService,
                              ITagService tagService,
                              IProductTagService tagProductTagService,
                              ICategoryService categoryService,
                              AppDbContext context)
        {

            _context = context;
            _sliderService = sliderService;
            _infoService = infoService;
            _userManager = userManager;
            _bannerService = bannerService;
            _tagService = tagService;
            _colorService = colorService;
            _tagProductService = tagProductTagService;
            _brandService = brandService;
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            ViewBag.Tags = await _tagService.GetAllSelectListAsync();

            HomeVM model = new()
            {
                Products = await _productService.GetAllAsync(),
                Categories = await _categoryService.GetAllAsync(),
                Brands = await _brandService.GetAllAsync(),
                Explores = await _context.Explores.ToListAsync(),
                Colors = await _colorService.GetAllAsync(),
                WishlistProducts = await _context.WishlistProducts.Where(bp => bp.Wishlist.AppUserId == userId)
                                        .ToListAsync(),
                Wishlists = await _context.Wishlists.ToListAsync(),
                ProductColors = await _context.ProductColors.ToListAsync(),
                InfoBanner = await _infoService.GetAllAsync(),
                Elementors = await _context.Elementors.ToListAsync(),
                ProductTags = await _context.ProductTags.ToListAsync(),
                Tags = await _tagService.GetAllAsync(),
                Banners = await _bannerService.GetAllAsync(),
                Baskets = await _context.BasketProducts
                                        .Where(bp => bp.Basket.AppUserId == userId)
                                        .ToListAsync(),
                Sliders = await _sliderService.GetAllAsync()
            };

            return View(model);
        }


        //[Route("/StatusCodeError/{statusCode}")]
        //public IActionResult Error(int statusCode)
        //{
        //    if (statusCode == 404)
        //    {
        //        ViewBag.ErrorMessage = "Page could not be found !";
        //    }

        //    return View("Error");

        //}


        public IActionResult ErrorPage(string error = "Internal Server Error")
        {
            return View(model: error);
        }


    }
}