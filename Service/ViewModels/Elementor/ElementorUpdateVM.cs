using Microsoft.AspNetCore.Http;

namespace Service.ViewModels.Elementor
{
    public class ElementorUpdateVM
    {

        public int Id { get; set; }
        public string? Image { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
