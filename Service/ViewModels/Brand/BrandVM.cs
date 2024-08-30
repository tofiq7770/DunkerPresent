using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.Brand
{
    public class BrandVM
    {

        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public IFormFile? Image { get; set; }
    }
}
