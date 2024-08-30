using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.InfoBanner
{
    public class InfoBannerUpdateVM
    {

        public int? Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Subtitle { get; set; }
        public string? Image { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
