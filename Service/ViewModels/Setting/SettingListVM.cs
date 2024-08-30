using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.Setting
{
    public class SettingListVM
    {
        public int? Id { get; set; }

        [Required]
        public string Key { get; set; }

        [Required]
        public string Value { get; set; }
    }
}
