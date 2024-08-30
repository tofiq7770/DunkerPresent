using Domain.Entities;
using Service.ViewModels.Brand;
using Service.ViewModels.Category;
using Service.ViewModels.Color;
using Service.ViewModels.Tag;

namespace DunkerFinal.ViewModels.Shop
{
    public class ShopVM
    {
        public Product Product { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<CategoryListVM> Categories { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
        public IEnumerable<BrandListVM> Brands { get; set; }
        public IEnumerable<ColorListVM> Colors { get; set; }
        public IEnumerable<ProductColor> ProductColors { get; set; }
        public IEnumerable<ProductTag> ProductTags { get; set; }
        public IEnumerable<TagListVM> Tags { get; set; }

        public AppUser AppUser { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public IEnumerable<BasketProduct> Baskets { get; set; }
        public IEnumerable<WishlistProduct> WishlistProducts { get; set; }
        public IEnumerable<Wishlist> Wishlists { get; set; }
    }
}
