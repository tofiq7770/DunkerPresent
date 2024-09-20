using System.ComponentModel.DataAnnotations;


namespace Service.ViewModels.Account
{
    public class RegisterVM
    {

        [Required]
        [TrimmedStringLength(100, 3, ErrorMessage = "Username must be between 3 and 100 characters long.")]
        public string Username { get; set; }

        [Required]
        [TrimmedStringLength(100, 3, ErrorMessage = "Fullname must be at least 3 characters long.")]
        public string Fullname { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

    }
}
public class TrimmedStringLengthAttribute : ValidationAttribute
{
    private readonly int _minLength;
    private readonly int _maxLength;

    public TrimmedStringLengthAttribute(int maxLength, int minLength = 0)
    {
        _minLength = minLength;
        _maxLength = maxLength;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("This field is required.");
        }

        var stringValue = value.ToString().Trim();

        if (stringValue.Length < _minLength)
        {
            return new ValidationResult($"The field must be at least {_minLength} characters long");
        }

        if (stringValue.Length > _maxLength)
        {
            return new ValidationResult($"The field must not exceed {_maxLength} characters.");
        }

        return ValidationResult.Success;
    }
}
