using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.Account
{
    public class ResetPasswordVM
    {
        [Required, DataType(DataType.Password), MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }

        [Required, DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        public string UserId { get; set; }
        public string Token { get; set; }
    }
}
