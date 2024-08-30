using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.Brand
{
    public class BrandCreateVM
    {

        [Required]
        public string Name { get; set; }
        [Required]
        public IFormFile? Image { get; set; }
    }
}
