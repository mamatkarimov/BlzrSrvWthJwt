using System.ComponentModel.DataAnnotations;

namespace Backend.API.Domain.Entities
{
    /// <summary>
    /// Управление отделением в болница
    /// </summary>
    public class DepartmentDto
    {  
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }
                
        public int? HeadDoctorId { get; set; }

        public string? HeadDoctorName { get; set; }
        public int WardCount { get; set; }
    }

    public class DepartmentRegisterInput
    {        
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public int? HeadDoctorId { get; set; }
    }

    /// <summary>
    /// Отделение в болница
    /// </summary>
    public class WardDto
    {
        public int Id { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public string WardNumber { get; set; }

        [Required]
        [Range(1, 100)]
        public int Capacity { get; set; }

        [Required]
        [RegularExpression("^[MNF]$", ErrorMessage = "Gender must be 'M', 'F', or 'N'")]
        public char GenderSpecific { get; set; }

        public string? DepartmentName { get; set; }
        public int BedCount { get; set; }
    }

    

    public class WardRegisterInput {
        public string WardNumber { get; set; } = default!;
        public int Capacity { get; set; }
        public char GenderSpecific { get; set; } // 'M', 'F', or 'N'
        public int DepartmentId { get; set; }
    }

    /// <summary>
    /// Койка в отделение
    /// </summary>
    public class BedDto
    {
        public int Id { get; set; }

        public string BedNumber { get; set; }

        public bool IsOccupied { get; set; } = false;

        public int WardId { get; set; }

        public string WardNumber { get; set; }
    }

    public class BedRegisterInput
    {
        public string BedNumber { get; set; } = default!;
        public int WardId { get; set; }
    }

}