using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.InfoBanner
{
    public class InfoBannerVM
    {
        public int? Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string Subtitle { get; set; }
        [Required]
        public IFormFile? Image { get; set; }
    }
}
