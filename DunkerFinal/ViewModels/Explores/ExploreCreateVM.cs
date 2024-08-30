using System.ComponentModel.DataAnnotations;

namespace DunkerFinal.ViewModels.Explores
{
    public class ExploreCreateVM
    {

        public int Id { get; set; }

        [Required]
        [MaxLength(25, ErrorMessage = "Max Length is 25")]
        public string Name { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public IFormFile? Image { get; set; }
    }
}
