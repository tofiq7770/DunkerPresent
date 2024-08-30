using Domain.Entities;

namespace Service.ViewModels.Product
{
    public class ProductDetailVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public string SKU { get; set; }

        public int Rating { get; set; } = 5;

        public int Quantity { get; set; }
        public decimal Weight { get; set; }
        public int CategoryId { get; set; }

        public string Category { get; set; }

        public int BrandId { get; set; }
        public string Brand { get; set; }


        public List<ProductTag> Tags { get; set; }
        public List<ProductColor> Colors { get; set; }

        public IEnumerable<ProductImage> ProductImages { get; set; }
    }
}
