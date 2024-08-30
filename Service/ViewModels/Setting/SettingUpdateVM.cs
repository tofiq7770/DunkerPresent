using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.Setting
{
    public class SettingUpdateVM
    {
        public int? Id { get; set; }
        [Required]
        public string Key { get; set; }

        [Required]
        public string Value { get; set; }
    }
}
