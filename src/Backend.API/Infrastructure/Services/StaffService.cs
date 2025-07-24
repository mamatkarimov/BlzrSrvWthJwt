using Backend.API.Application.DTOs;
using Backend.API.Application.Interfaces;
using Backend.API.Data;
using Backend.API.Domain.Entities;
using Backend.API.Interfaces;
using Backend.API.Models;
using Backend.API.Services;
using Backend.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Backend.API.Infrastructure.Services
{
    public class StaffService : IStaffService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAccessControlService _accessControlService;
        private readonly ILogger<StaffService> _logger;

        public StaffService(ApplicationDbContext context, IAccessControlService accessControlService, ILogger<StaffService> logger)
        {
            _context = context;
            _accessControlService = accessControlService;
        }

        public async Task<List<StaffDto>> GetAllAsync()
        {
            return await _context.StaffProfiles
                .Include(s => s.Department)
               .Select(p => new StaffDto
               {
                   Id = p.Id,
                   FirstName = p.FirstName,
                   LastName = p.LastName,
                   Position = p.Position,                   
                   Department = p.Department.Name,
                   UserId = p.UserId
               })
               .ToListAsync();
        }

        public async Task<StaffDto> GetByIdAsync(int id)
        {
            var staff = await _context.StaffProfiles.FindAsync(id);

            if (staff == null || !staff.IsActive)
            {
                return null;
            }

            return new StaffDto()
            {
                Id = staff.Id,
                FirstName = staff.FirstName,
                LastName = staff.LastName,
                Position = staff.Position,
                Department = staff.Department.Name,
                UserId = staff.UserId
            };
        }

        public async Task<BaseServiceResult<List<string>>> CreateAsync(StaffRegisterInput input)
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

            StaffProfile staff = new StaffProfile()
            {
                FirstName = input.FirstName,
                LastName = input.LastName,
                DepartmentId = input.DepartmentId,
                Position = input.Position,
                IsActive = true,
                UserId = result.Data[0]
            };

            _context.StaffProfiles.Add(staff);
            _context.SaveChanges();

            return BaseServiceResult<List<string>>.Success([staff.Id.ToString()]);
        }
    }
}
