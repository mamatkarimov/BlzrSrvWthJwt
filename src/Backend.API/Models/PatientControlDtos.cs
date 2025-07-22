using System.ComponentModel.DataAnnotations;

namespace Backend.API.Models
{
    public class PatientRegisterInput
    {     
            public string FirstName { get; set; } = default!;
            public string LastName { get; set; } = default!;
            public DateTime DateOfBirth { get; set; }
            public string Gender { get; set; } = default!;
            public Guid CreatedById { get; set; }
            public bool IsActive { get; set; } = true;
            public string Email { get; set; }
            public string UserName { get; set; }
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
       
       
    }
}
