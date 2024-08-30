using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.Tag
{
    public class TagVM
    {
        [Required]
        public string Name { get; set; }
    }
}
