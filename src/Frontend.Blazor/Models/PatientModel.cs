using System.ComponentModel.DataAnnotations;

namespace Frontend.Blazor.Models
{
    public class RegistrationResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public bool RequiresLogin { get; set; }
    }

    public class PatientRegistrationDto
    {
        public Guid PatientId { get; set; }
        public string Username { get; set; }
        public string TemporaryPassword { get; set; }
        public string RegistrationNote { get; set; }
    }

    public class PatientRegisterInput
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        [AgeValidation(18, ErrorMessage = "Patient must be at least 18 years old")]
        public DateTime DateOfBirth { get; set; } = DateTime.Today.AddYears(-18);

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Username must be between 4 and 20 characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }

    public class AgeValidationAttribute : ValidationAttribute
    {
        private readonly int _minimumAge;

        public AgeValidationAttribute(int minimumAge)
        {
            _minimumAge = minimumAge;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dateOfBirth)
            {
                if (dateOfBirth.AddYears(_minimumAge) > DateTime.Today)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}
