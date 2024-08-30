using Domain.Common;

namespace Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        public decimal Price { get; set; }
        public decimal Weight { get; set; }
        public string SKU { get; set; }
        public int Quantity { get; set; }
        public int? Rating { get; set; } = 0;
        public ICollection<Review>? Reviews { get; set; }
        public List<ProductColor>? ProductColors { get; set; }
        public ICollection<BasketProduct> BasketProducts { get; set; }
        public ICollection<WishlistProduct> WishlistProducts { get; set; }
        public List<ProductTag>? ProductTags { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }
    }
}
