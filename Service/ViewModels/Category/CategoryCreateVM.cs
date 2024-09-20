using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.Category
{
    public class CategoryCreateVM
    {
        public int? Id { get; set; }

        private string _names = null!;

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters.")]
        public string Name
        {
            get => _names;
            set => _names = value.Trim();
        }
    }
}
