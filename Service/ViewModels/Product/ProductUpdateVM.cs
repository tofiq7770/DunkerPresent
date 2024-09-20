using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Service.ViewModels.ProductColorVms;
using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.Product
{
    public class ProductUpdateVM
    {
        public int Id { get; set; }

        private string _name = null!;
        private string _desc = null!;

        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters.")]
        public string Name
        {
            get => _name;
            set => _name = value.Trim();
        }

        [StringLength(500, MinimumLength = 3, ErrorMessage = "Description must be between 3 and 500 characters.")]
        public string Description
        {
            get => _desc;
            set => _desc = value.Trim();
        }

        [Range(0.01, 100000, ErrorMessage = "Price must be a positive value and cannot exceed 100000..")]
        public decimal Price { get; set; }

        public string SKU { get; set; }

        [Range(0.01, 1000, ErrorMessage = "Weight must be a positive value and cannot exceed 1000..")]
        public decimal Weight { get; set; }
        [Range(0, 5)]
        public int? Rating { get; set; } = 5;

        [Range(0.01, 100000, ErrorMessage = "Quantity must be a positive value and cannot exceed 100000..")]
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
