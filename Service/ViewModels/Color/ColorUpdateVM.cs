using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.Color
{
    public class ColorUpdateVM
    {

        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
