namespace Backend.API.Models;

public class PatientVM
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; }

    public Guid? UserId { get; set; }
    public DateTime RegisteredAt { get; set; }
    public DateTime RegisteredBy { get; set; }

}

public class PatientListVM
{
    public List<PatientVM> Patients { get; set; }
    public int TotalRow { get; set; }
}