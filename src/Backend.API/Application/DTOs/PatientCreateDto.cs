using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.API.Application.DTOs
{
    public class PatientCreateDto
    {        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        
    }   
}