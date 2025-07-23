using Backend.API.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Numerics;
using static Backend.API.Settings.PolicyTypes;

namespace Backend.API.Permissions;

public class Permission
{
    protected Permission()
    {
    }

    protected Permission(string key, string title, string value = "")
    {
        Nodes = new List<Permission>();
        Key = key;
        Title = title;
        Value = string.IsNullOrEmpty(value) ? key : value;
        IsChecked = false;
    }

    public string Value { get; }
    public string Title { get; private set; }

    [JsonIgnore] public string Key { get; }

    public bool IsChecked { get; set; }
    public ICollection<Permission> Nodes { get; }

    public Permission AddNode(string key, string title)
    {
        var item = new Permission(key, title, $"{Value}.{key}");
        Nodes.Add(item);
        return item;
    }
}

public class PermissionList
{
    private readonly List<Permission> _permissions;

    public PermissionList()
    {
        _permissions = new List<Permission>();
        _permissions.Add(new AdministrativePermission());
        _permissions.Add(new PatientPermission());
        _permissions.Add(new AppointmentPermission());
    }

    public ICollection<Permission> Permissions => new Collection<Permission>(_permissions);

    public Permission GetPermissionByKey(string key)
    {
        var sections = key.Split('.');
        var keys = new Stack<string>(sections.Reverse());
        return FindPermission(keys, _permissions);
    }

    private Permission FindPermission(Stack<string> keys, ICollection<Permission> permissions)
    {
        var st = keys.Pop();
        var t = permissions.FirstOrDefault(x => x.Key == st);
        if (t == null) return null;
        if (t.Nodes.Count == 0) return t;
        return FindPermission(keys, t.Nodes);
    }

    //public void SetPermissionEnable(string key)
    //{
    //    var sections = key.Split('.');
    //    var keys = new Stack<string>(sections.Reverse());
    //    var item = FindPermission(keys, _permissions);
    //    if (item != null) item.IsChecked = true;
    //}

    public void SetPermissionEnable(string key)
    {
        var item = GetPermissionByKey(key);
        if (item != null) item.IsChecked = true;
    }

    public IEnumerable<string> GetAllPermissionValues()
    {
        return _permissions.SelectMany(p => p.Flatten()).Select(p => p.Value);
    }
}

public static class PermissionExtensions
{
    public static IEnumerable<Permission> Flatten(this Permission root)
    {
        yield return root;

        foreach (var child in root.Nodes)
        {
            foreach (var descendant in child.Flatten())
            {
                yield return descendant;
            }
        }
    }
}

public class AdministrativePermission : Permission
{
    public AdministrativePermission() : base("Administrative", "Manage Users")
    {
        AddNode("ManageUsers", "Manage Users");
        AddNode("ViewUsers", "View Users");
    }

    public const string AdministrativeManageUser = "Administrative.ManageUsers";
    public const string AdministrativeViewUser = "Administrative.ViewUsers";
}

public class PatientPermission : Permission
{
    public PatientPermission() : base("Patient", "Patients")
    {
        AddNode("Register", "Register Patient");
        AddNode("Edit", "Edit Patient");
        AddNode("View", "View Patient");
    }

    public const string Register = "Patient.Register";
    public const string Edit = "Patient.Edit";
    public const string View = "Patient.View";
}
public class AppointmentPermission : Permission
{
    public AppointmentPermission() : base("Appointment", "Appointments")
    {
        AddNode("Create", "Create Appointment");
        AddNode("Edit", "Edit Appointment");
        AddNode("View", "View Appointments");
    }

    public const string Create = "Appointment.Create";
    public const string Edit = "Appointment.Edit";
    public const string View = "Appointment.View";
}

//CanViewPatients View patient list   All except Patient
//CanRegisterPatient  Register a new patient Admin, Reception
//CanEditPatient Edit patient info   Admin, Reception, Doctor
//CanDeletePatient    Delete patient records Admin
//CanViewAppointments View appointments Doctor, Nurse, Reception, Patient
//CanCreateAppointment Schedule appointment Reception, Doctor
//CanEditAppointment Change appointment Doctor, Reception
//CanManageUsers CRUD users and assign roles Admin
//CanAccessCashDesk   View/payment handling   Cashier
//CanViewLabResults   View lab results Doctor, Nurse, Lab, Patient
//CanEditLabResults Enter/edit lab results Lab
//CanApproveDiagnoses Finalize diagnosis ChefDoctor, Doctor
//CanDischargePatient Discharge patient Doctor, ChefDoctor
