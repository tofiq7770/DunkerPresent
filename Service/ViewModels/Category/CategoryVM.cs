using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.Category
{
    public class CategoryVM
    {
        [Required]
        public string Name { get; set; }
    }
}
