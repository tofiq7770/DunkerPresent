using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.Tag
{
    public class TagDetailVM
    {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
