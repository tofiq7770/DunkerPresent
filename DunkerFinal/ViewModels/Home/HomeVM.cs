using Domain.Entities;
using Service.ViewModels.Banner;
using Service.ViewModels.Brand;
using Service.ViewModels.Category;
using Service.ViewModels.Color;
using Service.ViewModels.InfoBanner;
using Service.ViewModels.Slider;
using Service.ViewModels.Tag;

namespace DunkerFinal.ViewModels.Home
{
    public class HomeVM
    {
        public IEnumerable<SliderListVM> Sliders { get; set; }
        public IEnumerable<BrandListVM> Brands { get; set; }
        public IEnumerable<BasketProduct> Baskets { get; set; }
        public IEnumerable<ColorListVM> Colors { get; set; }
        public IEnumerable<ProductColor> ProductColors { get; set; }
        public IEnumerable<ProductTag> ProductTags { get; set; }
        public IEnumerable<Explore> Explores { get; set; }
        public IEnumerable<WishlistProduct> WishlistProducts { get; set; }
        public IEnumerable<Wishlist> Wishlists { get; set; }
        public IEnumerable<TagListVM> Tags { get; set; }
        public IEnumerable<CategoryListVM> Categories { get; set; }
        public List<Elementor> Elementors { get; set; }
        public IEnumerable<InfoBannerListVM> InfoBanner { get; set; }
        public IEnumerable<BannerListVM> Banners { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
