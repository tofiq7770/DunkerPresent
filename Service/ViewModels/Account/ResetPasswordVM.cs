using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.Account
{
    public class ResetPasswordVM
    {
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?!.*\s).{6,}$", ErrorMessage = "Password must contain at least one digit, one uppercase letter, one lowercase letter, and be at least 6 characters long.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        public string UserId { get; set; }
        public string Token { get; set; }
    }
}
