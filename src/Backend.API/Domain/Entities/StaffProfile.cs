using Backend.API.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Backend.API.Domain.Entities
{
    public class StaffProfile
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public string? UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string Position { get; set; } // Example: "Doctor", "Nurse", "Admin", etc.
        public string Department { get; set; } // Optional: e.g., "Cardiology"
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;

        public virtual ICollection<Patient> CreatedPatients { get; set; }
    }

}