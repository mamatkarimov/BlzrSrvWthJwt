using System;
namespace Backend.API.Domain.Entities
{
    public class MedicalRecord
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; } = default!;
        public int DoctorId { get; set; }
        public StaffProfile Doctor { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public string Anamnesis { get; set; } = default!;
        public string Diagnosis { get; set; } = default!;
        public string Prescriptions { get; set; } = default!;
    }

}