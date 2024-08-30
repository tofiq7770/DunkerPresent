using Domain.Entities;
using DunkerFinal.ViewModels.Header;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.DAL;
using Service.Services.Interfaces;

namespace DunkerFinal.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly ISettingService _settingService;
        private readonly IProductService _productService;
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public HeaderViewComponent(ISettingService settingService, IProductService productService, UserManager<AppUser> userManager, AppDbContext context)
        {
            _settingService = settingService;
            _userManager = userManager;
            _productService = productService;
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var userId = _userManager.GetUserId((System.Security.Claims.ClaimsPrincipal)User);
            AppUser existUser = new();

            if (User.Identity.IsAuthenticated)
            {
                existUser = await _userManager.FindByNameAsync(User.Identity?.Name);
            }

            int quantity = await _context.BasketProducts.Where(m => m.Basket.AppUserId == existUser.Id).CountAsync();
            HeaderVM model = new()
            {
                Settings = await _settingService.GetAll(),
                BasketCount = quantity,
                Products = await _productService.GetAllAsync(),
                Baskets = await _context.BasketProducts
                                        .Where(bp => bp.Basket.AppUserId == userId)
                                        .ToListAsync()
            };

            return await Task.FromResult(View(model));
        }
    }

}
