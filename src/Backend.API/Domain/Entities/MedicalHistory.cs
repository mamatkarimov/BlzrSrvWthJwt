using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.API.Domain.Entities
{
public class MedicalHistory
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public Guid PatientId { get; set; }
    public Guid? AppointmentId { get; set; }
    
    public DateTime RecordDate { get; set; } = DateTime.UtcNow;
    
    [Required]
    public int RecordedByID { get; set; }
    
    [Required]
    public string HistoryType { get; set; } // Anamnesis, Allergy, Chronic Disease, etc.
    
    public string Description { get; set; }
    
    // Navigation properties
    public Patient Patient { get; set; }
    public Appointment Appointment { get; set; }  // Добавляем навигационное свойство
    public StaffProfile RecordedBy { get; set; }
}




}