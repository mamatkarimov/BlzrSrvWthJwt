using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.API.Domain.Entities
{
    public class Ward
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int DepartmentId { get; set; }
    
    [Required]
    public string WardNumber { get; set; }
    
    [Required]
    public int Capacity { get; set; }
    
    [Required]
    public char GenderSpecific { get; set; } // 'M', 'F', or 'N'
    
    // Navigation properties
    public Department Department { get; set; }
    public ICollection<Bed> Beds { get; set; }
}




}