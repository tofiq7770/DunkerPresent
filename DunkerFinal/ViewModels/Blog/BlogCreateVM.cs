using System.ComponentModel.DataAnnotations;

namespace DunkerFinal.ViewModels.Blog
{
    public class BlogCreateVM
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Max Length is 50")]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public IFormFile? Image { get; set; }
    }
}
