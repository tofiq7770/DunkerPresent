using System.ComponentModel.DataAnnotations;

namespace DunkerFinal.ViewModels.Teams
{
    public class TeamCreateVM
    {

        public int Id { get; set; }

        [Required]
        [MaxLength(25, ErrorMessage = "Max Length is 25")]
        public string Name { get; set; }
        [Required]
        public string Position { get; set; }
        [Required]
        public IFormFile? Image { get; set; }
    }
}
