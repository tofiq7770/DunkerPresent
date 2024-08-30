using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.Tag
{
    public class TagListVM
    {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
