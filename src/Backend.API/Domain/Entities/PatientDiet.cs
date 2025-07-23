using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.API.Domain.Entities
{
    public class PatientDiet
{

[Key]    public int Id { get; set; }
    
    [Required]
    public Guid PatientId { get; set; }
    
    [Required]
    public int HospitalizationId { get; set; }
    
    [Required]
    public string DietType { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    
    public DateTime? EndDate { get; set; }
    public string Notes { get; set; }
    
    // Navigation properties
    public Patient Patient { get; set; }
    public Hospitalization Hospitalization { get; set; }
}


}