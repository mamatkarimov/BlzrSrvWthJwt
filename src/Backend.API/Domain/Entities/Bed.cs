using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.API.Domain.Entities
{
    /// <summary>
    /// Койка в отделение
    /// </summary>
    public class Bed
{

[Key]
    public int Id { get; set; }
    
    [Required]
    public int WardId { get; set; }
    
    [Required]
    public string BedNumber { get; set; }
    
    [Required]
    public bool IsOccupied { get; set; } = false;
    
    // Navigation properties
    public Ward Ward { get; set; }
    public ICollection<Hospitalization> Hospitalizations { get; set; }
}


}