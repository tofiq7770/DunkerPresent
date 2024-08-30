using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.Category
{
    public class CategoryDetailVM
    {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
