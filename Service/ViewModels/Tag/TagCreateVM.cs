using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.Tag
{
    public class TagCreateVM
    {

        [Required]
        public string Name { get; set; }
    }
}
