using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.DAL;
using Service.ViewModels.Basket;
using Stripe.Checkout;

namespace DunkerFinal.Controllers
{
    public class CheckOutController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public CheckOutController(AppDbContext appDbContext, UserManager<AppUser> userManager)
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


            var domain = "https://localhost:44373/";
            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"Basket/OrderConfirmation",
                CancelUrl = domain + $"Basket/Index",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment"
            };

            foreach (var item in basketListVMs)
            {
                var sessionListItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions()
                        {
                            Name = item.Name.ToString(),
                        }
                    },
                    Quantity = item.Quantity
                };
                options.LineItems.Add(sessionListItem);
            };
            var service = new SessionService();
            Session session = service.Create(options);
            Response.Headers.Add("Location", session.Url);

            return new StatusCodeResult(303);
        }
    }
}
