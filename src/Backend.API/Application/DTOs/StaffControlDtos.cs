using Backend.API.Domain.Entities;
using Backend.API.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.API.Application.DTOs
{
    public class StaffDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string? UserId { get; set; }
        public string Position { get; set; } // Example: "Doctor", "Nurse", "Admin", etc.
        public string Department { get; set; } // Optional: e.g., "Cardiology"        

    }

    public class StaffRegisterInput
    {
        public int Id { get; set; }
        public string Position { get; set; } // Example: "Doctor", "Nurse", "Admin", etc.
        public int DepartmentId { get; set; } // Optional: e.g., "Cardiology"
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
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
