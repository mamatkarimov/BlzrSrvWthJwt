using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.API.Domain.Entities
{
    public class Invoice
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public Guid PatientId { get; set; }
    
    [Required]
    public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;
    
    public DateTime? DueDate { get; set; }
    
    [Required]
    public decimal TotalAmount { get; set; }
    
    [Required]
    public decimal PaidAmount { get; set; } = 0;
    
    [Required]
    public string Status { get; set; } = "Pending"; // Pending, PartiallyPaid, Paid, Cancelled
    
    [Required]
    public int CreatedById { get; set; }
    
    // Navigation properties
    public Patient Patient { get; set; }
    public StaffProfile CreatedBy { get; set; }
    public ICollection<InvoiceDetail> InvoiceDetails { get; set; }
    public ICollection<Payment> Payments { get; set; }
}


}