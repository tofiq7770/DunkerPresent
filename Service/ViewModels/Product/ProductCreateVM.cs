using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.Product
{
    public class ProductCreateVM
    {

        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }

        public string SKU { get; set; }

        [Range(0, 5)]
        public int Rating { get; set; } = 5;

        public int Quantity { get; set; }
        public decimal Weight { get; set; }

        public int CategoryId { get; set; }

        public int BrandId { get; set; }

        public List<int> TagIds { get; set; } = null!;
        public List<int> ColorIds { get; set; } = null!;

        public IFormFile[] Images { get; set; } = null!;
    }
}
