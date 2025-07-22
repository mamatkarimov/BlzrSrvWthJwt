using Backend.API.Application.DTOs;
using Backend.API.Data;
using Backend.API.Domain.Entities;
using Backend.API.Entities;
using Backend.API.Interfaces;
using Backend.API.Models;
using Backend.API.Services;
using Backend.API.Settings;
using Backend.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using static Backend.API.Settings.PolicyTypes;

namespace Backend.API.Infrastructure.Services
{
    public class PatientService : IPatientService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAccessControlService _accessControlService;
        private readonly ILogger<PatientService> _logger;

        public PatientService(ApplicationDbContext context, IAccessControlService accessControlService, ILogger<AccessControlService> logger)
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

        public async Task<PatientDto> GetByIdAsync(Guid id)
        {
            var patient = await _context.Patients.FindAsync(id);

            if (patient == null || !patient.IsActive)
            {
                return null;
            }

            return new PatientDto()
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender,
                UserId = patient.UserId
            };
        }

        public async Task<BaseServiceResult<List<string>>> CreateAsync(PatientRegisterInput input)
        {
            var user = new UserRegisterInput
            {
                UserName = input.UserName,
                Email = input.Email,
                Name = input.FirstName,
                Family = input.LastName,
                Password = input.Password,
                ConfirmPassword = input.ConfirmPassword,
                Role = [UserRoles.Patient]
            };

            var result = await _accessControlService.CreateUser(user);
            if (!result.Succeeded)
            {
                return result;
            }

            Patient patient = new Patient()
            {
                Id = Guid.NewGuid(),
                FirstName = input.FirstName,
                LastName = input.LastName,
                DateOfBirth = input.DateOfBirth,
                //CreatedById = user.Id,
                Gender = input.Gender,
                IsActive = true,
                UserId = result.Data[0]
            };

            _context.Patients.Add(patient);
            _context.SaveChanges();

            return BaseServiceResult<List<string>>.Success([patient.Id.ToString()]);

        }
    }
}
