using DunkerFinal.ViewModels.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.DAL;

namespace DunkerFinal.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Index()
        {
            var dashboardData = new DashboardVM
            {
                ProductCount = await _context.Products.CountAsync(),
                CategoryCount = await _context.Categories.CountAsync(),
                BrandCount = await _context.Brands.CountAsync(),
                TagCount = await _context.Tags.CountAsync(),
                ColorCount = await _context.Colors.CountAsync(),


                ColorLabels = new List<string>(),
                ProductColors = new List<int>(),
                TagLabels = new List<string>(),
                ProductTags = new List<int>()
            };

            var colorCounts = await _context.Colors
           .Select(c => new
           {
               ColorName = c.Name,
               ProductCount = c.ProductColors.Count()
           })
           .ToListAsync();

            dashboardData.ColorLabels = colorCounts.Select(cc => cc.ColorName).ToList();
            dashboardData.ProductColors = colorCounts.Select(cc => cc.ProductCount).ToList();

            // Fetch tag data and counts
            var tagCounts = await _context.Tags
                .Select(t => new
                {
                    TagName = t.Name,
                    ProductCount = t.ProductTags.Count()
                })
                .ToListAsync();

            dashboardData.TagLabels = tagCounts.Select(tc => tc.TagName).ToList();
            dashboardData.ProductTags = tagCounts.Select(tc => tc.ProductCount).ToList();

            return View(dashboardData);
        }
    }
}
