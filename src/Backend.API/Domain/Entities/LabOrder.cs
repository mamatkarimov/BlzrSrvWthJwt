using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.API.Domain.Entities
{
    public class LabOrder
{
    [Key] 
    public int Id { get; set; }
    
    [Required]
    public Guid PatientId { get; set; }
    
    [Required]
    public int OrderedById { get; set; }
    
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    
    [Required]
    public string Status { get; set; } = "Pending"; // Pending, InProgress, Completed, Cancelled
    
    [Required]
    public string Priority { get; set; } = "Routine"; // Routine, Urgent, STAT
    
    public string Notes { get; set; }
    
    // Navigation properties
    public Patient Patient { get; set; }
    public StaffProfile OrderedBy { get; set; }
    
    public ICollection<LabOrderDetail> LabOrderDetails { get; set; } = new List<LabOrderDetail>();
}




}