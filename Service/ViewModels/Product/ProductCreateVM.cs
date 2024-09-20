using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.Product
{
    public class ProductCreateVM
    {

        private string _names = null!;
        private string _descs = null!;

        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters.")]
        public string Name
        {
            get => _names;
            set => _names = value.Trim();
        }

        [StringLength(500, MinimumLength = 3, ErrorMessage = "Description must be between 3 and 500 characters.")]
        public string Description
        {
            get => _descs;
            set => _descs = value.Trim();
        }
        [Range(0.01, 100000, ErrorMessage = "Price must be a positive value and cannot exceed 100000..")]
        public decimal Price { get; set; }

        public string SKU { get; set; }

        [Range(0, 5)]
        public int Rating { get; set; } = 5;

        [Range(0.01, 100000, ErrorMessage = "Quantity must be a positive value and cannot exceed 100000..")]
        public int Quantity { get; set; }
        [Range(0.01, 1000, ErrorMessage = "Weight must be a positive value and cannot exceed 1000..")]
        public decimal Weight { get; set; }

        public int CategoryId { get; set; }

        public int BrandId { get; set; }

        public List<int> TagIds { get; set; } = null!;
        public List<int> ColorIds { get; set; } = null!;

        public IFormFile[] Images { get; set; } = null!;
    }
}
