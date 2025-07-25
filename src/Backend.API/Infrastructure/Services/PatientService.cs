﻿using Backend.API.Application.DTOs;
using Backend.API.Application.Interfaces;
using Backend.API.Data;
using Backend.API.Domain.Entities;
using Backend.API.Entities;
using Backend.API.Interfaces;
using Backend.API.Models;
using Backend.API.Services;
using Backend.API.Settings;
using Backend.Domain.Enums;
using BSMed.Shared;
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
        private readonly IUserContext _userContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public PatientService(ApplicationDbContext context, IAccessControlService accessControlService, ILogger<PatientService> logger, IUserContext userContext, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _accessControlService = accessControlService;
            _logger = logger;
            _userContext = userContext;
            _userManager = userManager;
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
            if (string.IsNullOrEmpty(_userContext.UserId))
            {
                throw new UnauthorizedAccessException("User not authenticated");
            }

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
                CreatedById = _userContext.UserId,
                Gender = input.Gender,
                IsActive = true,
                UserId = result.Data[0]
            };

            _context.Patients.Add(patient);
            _context.SaveChanges();

            return BaseServiceResult<List<string>>.Success([patient.Id.ToString()]);

        }

        private string GenerateUsername(string firstName, string lastName)
        {
            var baseUsername = $"{firstName[0]}{lastName}".ToLower();
            var username = baseUsername;
            var counter = 1;

            while (_userManager.Users.Any(u => u.UserName == username))
            {
                username = $"{baseUsername}{counter++}";
            }

            return username;
        }

        private string GenerateTemporaryPassword()
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghjkmnpqrstuvwxyz23456789!@$?_-";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 10)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task<PatientRegistrationResult> RegisterPatientAsync(PatientRegisterInput input, string createdByStaffId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Generate username if not provided
                var username = input.UserName ?? GenerateUsername(input.FirstName, input.LastName);

                // Create user account
                var user = new ApplicationUser
                {
                    UserName = username,
                    Email = input.Email,
                    EmailConfirmed = true // Skip verification for staff-created accounts
                };

                // Generate a temporary password
                var tempPassword = GenerateTemporaryPassword();

                var createUserResult = await _userManager.CreateAsync(user, tempPassword);
                if (!createUserResult.Succeeded)
                    return PatientRegistrationResult.Failure(createUserResult.Errors);

                // Add to Patient role
                await _userManager.AddToRoleAsync(user, "Patient");

                // Create patient profile
                var patient = new Patient
                {
                    UserId = user.Id,
                    FirstName = input.FirstName,
                    LastName = input.LastName,
                    DateOfBirth = input.DateOfBirth,
                    Gender = input.Gender,
                    CreatedById = createdByStaffId,
                    //CreatedAt = DateTime.UtcNow
                };

                await _context.Patients.AddAsync(patient);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return PatientRegistrationResult.Success(patient.Id, username, tempPassword);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error creating patient");
                return PatientRegistrationResult.Failure("An error occurred while creating the patient");
            }
        }

        public async Task<IdentityResult> SelfCreateAsync(PatientRegisterInput input)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Create user account first
                var user = new ApplicationUser
                {
                    UserName = input.UserName,
                    Email = input.Email,
                    EmailConfirmed = true // Or set to false and implement email confirmation
                };

                var createUserResult = await _userManager.CreateAsync(user, input.Password);

                if (!createUserResult.Succeeded)
                    return createUserResult;

                // Add to Patient role
                await _userManager.AddToRoleAsync(user, "Patient");

                // Create patient profile
                var patient = new Patient
                {
                    UserId = user.Id,
                    FirstName = input.FirstName,
                    LastName = input.LastName,
                    DateOfBirth = input.DateOfBirth,
                    Gender = input.Gender,
                 //   CreatedAt = DateTime.UtcNow
                };

                await _context.Patients.AddAsync(patient);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error creating patient");
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "An error occurred while creating the patient"
                });
            }
        }
    }
}
