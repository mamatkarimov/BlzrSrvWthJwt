using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Backend.API.Application.DTOs 
{
    public class PatientDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }

        public string? UserId { get; set; }
    }

    public class PatientRegisterInput
    {     
            public string FirstName { get; set; } = default!;
            public string LastName { get; set; } = default!;
            public DateTime DateOfBirth { get; set; }
            public string Gender { get; set; } = default!;            
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

    public class PatientRegistrationResult
    {
        public bool IsSuccess { get; }
        public Guid PatientId { get; }
        public string Username { get; }
        public string TemporaryPassword { get; }
        public List<string> Errors { get; }

        private PatientRegistrationResult(bool success, Guid patientId,
            string username, string tempPassword, List<string> errors)
        {
            IsSuccess = success;
            PatientId = patientId;
            Username = username;
            TemporaryPassword = tempPassword;
            Errors = errors;
        }

        public static PatientRegistrationResult Success(Guid patientId, string username, string tempPassword)
            => new(true, patientId, username, tempPassword, null);

        public static PatientRegistrationResult Failure(IEnumerable<IdentityError> errors)
            => new(false, Guid.Empty, null, null, errors.Select(e => e.Description).ToList());

        public static PatientRegistrationResult Failure(string error)
            => new(false, Guid.Empty, null, null, new List<string> { error });
    }

    public class StaffPatientRegisterInput : PatientRegisterInput
    {
        public string RegistrationNote { get; set; }
    }

    public class PatientRegistrationDto
    {
        public Guid PatientId { get; set; }
        public string Username { get; set; }
        public string TemporaryPassword { get; set; }
        public string RegistrationNote { get; set; }
    }
}
