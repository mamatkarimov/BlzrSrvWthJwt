using Backend.API.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.API.Domain.Entities
{
    /// <summary>
    /// Управление отделением в болница
    /// </summary>
    public class Department
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        public int? HeadDoctorId { get; set; }

        // Navigation properties
        [ForeignKey("HeadDoctorId")]
        public StaffProfile HeadDoctor { get; set; }

        public virtual ICollection<StaffProfile> StaffMembers { get; set; }
        public virtual ICollection<Ward> Wards { get; set; }
        public virtual ICollection<PatientQueue> PatientQueues { get; set; } = new List<PatientQueue>();
    }


}