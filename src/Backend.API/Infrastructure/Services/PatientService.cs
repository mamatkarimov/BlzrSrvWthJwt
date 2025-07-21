using Backend.API.Application.DTOs;
using Backend.API.Data;
using Backend.API.Domain.Entities;
using Backend.API.Models;
using Backend.API.Services;
using Backend.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.Arm;

namespace Backend.API.Infrastructure.Services
{
    public class PatientService : IPatientService
    {
        private readonly ApplicationDbContext _context;
        private readonly AccessControlService _accessControlService;

        public PatientService(ApplicationDbContext context, AccessControlService accessControlService)
        {
            _context = context;
            _accessControlService = accessControlService;
        }

        public async Task<List<PatientDto>> GetAllAsync()
        {
            return await _context.Patients
               .Select(p => new PatientDto
               {
                   Id = p.Id,
                   FirstName = p.FirstName,
                   LastName = p.LastName,
                   DateOfBirth = p.DateOfBirth,
                   Gender = p.Gender,
                   UserId = p.UserId
               })
               .ToListAsync();
        }

        public async Task<PatientDto> GetPatient(Guid id)
        {
            var patient = await _context.Patients.FindAsync(id);

            if (patient == null || !patient.IsActive)
            {
                return null;
            }

            return new PatientDto() { 
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender,
                UserId = patient.UserId
            };
        }

        public async Task<PatientDto> CreateAsync(PatientCreateDto input)
        {
            var userId = User.GetUserId<Guid>();

            await _accessControlService.CreateUser(new UserRegisterInput
            {
                Email = input.Email,
                UserName = input.Username,
                Password = input.Password,
                Name = input.FirstName,
                Role = new[] { UserRoles.Patient }
            });
            
            
            var patient = new Patient
            {
                Id = Guid.NewGuid(),
                FirstName = input.FirstName,
                LastName = input.LastName,
                DateOfBirth = input.DateOfBirth,
                Gender = input.Gender
            };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            return new PatientDto
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender
            };
        }
    }
}
