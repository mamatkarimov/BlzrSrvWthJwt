using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.API.Domain.Entities
{
    public class Hospitalization
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid PatientId { get; set; }

        [Required]
        public int BedId { get; set; }

        [Required]
        public DateTime AdmissionDate { get; set; }

        public DateTime? DischargeDate { get; set; }
        public string DiagnosisOnAdmission { get; set; }
        public string DiagnosisOnDischarge { get; set; }
        public int AttendingDoctorId { get; set; }

        [Required]
        public string Status { get; set; } // Active, Discharged, Transferred

        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }
        [ForeignKey("BedId")]
        public Bed Bed { get; set; }
        [ForeignKey("AttendingDoctorId")]
        public StaffProfile AttendingDoctor { get; set; }
        public ICollection<NurseRound> NurseRounds { get; set; }
        public ICollection<PatientDiet> PatientDiets { get; set; }
    }


}