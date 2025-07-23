using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.API.Domain.Entities
{
    public class Prescription
{

[Key]
    public int Id { get; set; }
    
    [Required]
    public Guid PatientId { get; set; }
    
    [Required]
    public int PrescribedById { get; set; }
    
    public DateTime PrescriptionDate { get; set; } = DateTime.UtcNow;
    
    [Required]
    public string Medication { get; set; }
    
    public string Dosage { get; set; }
    public string Frequency { get; set; }
    public string Duration { get; set; }
    public string Instructions { get; set; }
    
    [Required]
    public string Status { get; set; } = "Active";
    
    // Navigation properties
    public Patient Patient { get; set; }
    public StaffProfile PrescribedBy { get; set; }
}




}