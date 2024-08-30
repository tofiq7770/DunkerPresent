using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.Elementor
{
    public class ElementorCreateVM
    {

        [Required]
        public IFormFile? Image { get; set; }
    }
}
