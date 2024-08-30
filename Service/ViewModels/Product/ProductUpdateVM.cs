using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Service.ViewModels.ProductColorVms;
using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.Product
{
    public class ProductUpdateVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string SKU { get; set; }

        public decimal Weight { get; set; }
        [Range(0, 5)]
        public int? Rating { get; set; } = 5;

        public int Quantity { get; set; }

        public int CategoryId { get; set; }

        public int BrandId { get; set; }

        public List<int>? TagIds { get; set; }
        public List<int>? ColorIds { get; set; }

        public IEnumerable<ProductColorListVM>? Colors { get; set; }
        public IFormFile[]? Images { get; set; }
        public IEnumerable<ProductImage>? ProductImages { get; set; }
    }
}
