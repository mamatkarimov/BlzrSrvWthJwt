using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.API.Application.DTOs
{
    public class PatientDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
       
        public string? UserId { get; set; }
    }   
}