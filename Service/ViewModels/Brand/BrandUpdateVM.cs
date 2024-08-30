using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.Brand
{
    public class BrandUpdateVM
    {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Image { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
