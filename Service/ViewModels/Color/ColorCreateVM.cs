using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.Color
{
    public class ColorCreateVM
    {

        [Required]
        public string Name { get; set; }
    }
}
