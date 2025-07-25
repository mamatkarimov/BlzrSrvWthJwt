using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Reflection;

namespace BSMed.Shared;

public enum Gender
{
    [Display(Name = "Male")]
    Male,
    [Display(Name = "Female")]
    Female,
    [Display(Name = "Other")]
    Other,
    [Display(Name = "Unknown")]
    Unknown
}

public enum Department
{
    [Display(Name = "Cardiology")]
    Cardiology,
    [Display(Name = "Neurology")]
    Neurology,
    [Display(Name = "Orthopedics")]
    Orthopedics,
    [Display(Name = "Pediatrics")]
    Pediatrics
}

public static class StaticDataService
{
    public static List<GenderOption> Genders { get; } = Enum.GetValues(typeof(Gender))
        .Cast<Gender>()
        .Select(g => new GenderOption
        {
            Value = g.ToString(),
            DisplayName = g.GetDisplayName()
        })
        .ToList();

    public static List<DepartmentOption> Departments { get; } = Enum.GetValues(typeof(Department))
        .Cast<Department>()
        .Select(d => new DepartmentOption
        {
            Value = d.ToString(),
            DisplayName = d.GetDisplayName()
        })
        .ToList();
}

// Extension method to get DisplayName attribute
public static class EnumExtensions
{
    public static string GetDisplayName(this Enum enumValue)
    {
        return enumValue.GetType()
            .GetMember(enumValue.ToString())
            .First()
            .GetCustomAttribute<DisplayAttribute>()?
            .Name ?? enumValue.ToString();
    }
}

public class GenderOption
{
    public string Value { get; set; }
    public string DisplayName { get; set; }
}

public class DepartmentOption
{
    public string Value { get; set; }
    public string DisplayName { get; set; }
}

