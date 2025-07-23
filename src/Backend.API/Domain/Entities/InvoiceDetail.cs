using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.API.Domain.Entities
{
    public class InvoiceDetail
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public Guid InvoiceId { get; set; }
    
    [Required]
    public Guid ServiceId { get; set; }
    
    [Required]
    public int Quantity { get; set; } = 1;
    
    [Required]
    public decimal UnitPrice { get; set; }
    
    [Required]
    public decimal Discount { get; set; } = 0;
    
    // Navigation properties
    public Invoice Invoice { get; set; }
    public Service Service { get; set; }
}


}