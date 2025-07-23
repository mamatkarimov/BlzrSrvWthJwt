using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.API.Domain.Entities
{
    public class NurseRound
{

[Key]
    public int Id { get; set; }
    
    [Required]
    public int NurseId { get; set; }
    
    [Required]
    public Guid PatientId { get; set; }
    
    public DateTime RoundDate { get; set; } = DateTime.UtcNow;
    public decimal? Temperature { get; set; }
    public string BloodPressure { get; set; }
    public int? Pulse { get; set; }
    public int? RespirationRate { get; set; }
    public string Notes { get; set; }
    
    // Navigation properties
    public StaffProfile Nurse { get; set; }
    public Patient Patient { get; set; }
}


}