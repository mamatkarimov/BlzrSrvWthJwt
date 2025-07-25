using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.API.Domain.Entities
{
    public class PatientQueue
    {
        [Key]
        public int Id { get; set; }
                [Required]
        public Guid PatientId { get; set; }
        
        public Guid? AppointmentId { get; set; }
        
        public DateTime QueueDate { get; set; } = DateTime.UtcNow;
        
        [Required]
        public string Status { get; set; } = "Waiting"; // Waiting, InProgress, Completed, Cancelled
        
        [Required]
        public int Priority { get; set; } = 5; // 1 highest, 10 lowest
        
        public int? DepartmentId { get; set; }
        public string Notes { get; set; }
        
        // Navigation properties
        public Patient Patient { get; set; }
        public Appointment Appointment { get; set; }
        public Department Department { get; set; }
    }  

   
}