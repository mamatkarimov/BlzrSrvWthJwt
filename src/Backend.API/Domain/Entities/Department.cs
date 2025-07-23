using Backend.API.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.API.Domain.Entities
{
   
    public class Department
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        public int HeadDoctorId { get; set; }

        // Navigation properties
        public StaffProfile HeadDoctor { get; set; }
        public ICollection<Ward> Wards { get; set; }
        public ICollection<PatientQueue> PatientQueues { get; set; } = new List<PatientQueue>();
    }


}