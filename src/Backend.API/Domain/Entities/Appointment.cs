using Backend.API.Entities;
using System;
using System.Collections.Generic;
namespace Backend.API.Domain.Entities
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; } = default!;
        public int DoctorId { get; set; }
        public StaffProfile Doctor { get; set; } = default!;
        public DateTime AppointmentDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; } = "Scheduled";
        public string Symptoms { get; set; } = "";

        public ICollection<AssignedTest> AssignedTests { get; set; } = new List<AssignedTest>();
        public ICollection<PatientQueue> PatientQueues { get; set; } = new List<PatientQueue>();
        public ICollection<MedicalHistory> MedicalHistories { get; set; } = new List<MedicalHistory>();
    }   

}