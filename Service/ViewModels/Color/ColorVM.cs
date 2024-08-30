using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.Color
{
    public class ColorVM
    {
        [Required]
        public string Name { get; set; }
    }
}
