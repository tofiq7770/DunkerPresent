using Microsoft.AspNetCore.Http;

namespace Service.ViewModels.Elementor
{
    public class ElementorListVM
    {
        public int? Id { get; set; }
        public IFormFile? Image { get; set; }
    }
}
