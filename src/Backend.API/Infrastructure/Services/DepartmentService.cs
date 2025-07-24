using Backend.API.Application.DTOs;
using Backend.API.Application.Interfaces;
using Backend.API.Data;
using Backend.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.API.Infrastructure.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DepartmentService> _logger;

        public DepartmentService(ApplicationDbContext context, ILogger<DepartmentService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<DepartmentDto>> GetAllDepartmentsAsync()
        {
            try
            {
                return await _context.Departments
                    .AsNoTracking()
                    .Include(d => d.HeadDoctor)
                    .Include(d => d.Wards)
                    .Select(d => new DepartmentDto
                    {
                        Id = d.Id,
                        Name = d.Name,
                        Description = d.Description,
                        HeadDoctorId = d.HeadDoctorId,
                        HeadDoctorName = d.HeadDoctor != null ?
                            $"{d.HeadDoctor.FirstName} {d.HeadDoctor.LastName}" : null,
                        WardCount = d.Wards != null ? d.Wards.Count : 0
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all departments");
                throw;
            }
        }

        public async Task<DepartmentDto?> GetDepartmentByIdAsync(int id)
        {
            try
            {
                var department = await _context.Departments
                    .AsNoTracking()
                    .Include(d => d.HeadDoctor)
                    .Include(d => d.Wards)
                    .FirstOrDefaultAsync(d => d.Id == id);

                if (department == null)
                {
                    return null;
                }

                return new DepartmentDto
                {
                    Id = department.Id,
                    Name = department.Name,
                    Description = department.Description,
                    HeadDoctorId = department.HeadDoctorId,
                    HeadDoctorName = department.HeadDoctor != null ?
                        $"{department.HeadDoctor.FirstName} {department.HeadDoctor.LastName}" : null,
                    WardCount = department.Wards != null ? department.Wards.Count : 0
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting department with ID {id}");
                throw;
            }
        }

        public async Task<DepartmentDto> CreateDepartmentAsync(DepartmentDto departmentDto)
        {
            try
            {
                ValidateDepartmentDto(departmentDto);

                var headDoctor = await _context.StaffProfiles.FindAsync(departmentDto.HeadDoctorId);
                if (departmentDto.HeadDoctorId.HasValue && headDoctor == null)
                {
                    throw new InvalidOperationException($"Head doctor with ID {departmentDto.HeadDoctorId} not found");
                }

                if (await DepartmentNameExists(departmentDto.Name))
                {
                    throw new InvalidOperationException($"Department with name '{departmentDto.Name}' already exists");
                }

                var newDepartment = new Department
                {
                    Name = departmentDto.Name,
                    Description = departmentDto.Description,
                    HeadDoctorId = departmentDto.HeadDoctorId
                };

                _context.Departments.Add(newDepartment);
                await _context.SaveChangesAsync();

                return new DepartmentDto
                {
                    Id = newDepartment.Id,
                    Name = newDepartment.Name,
                    Description = newDepartment.Description,
                    HeadDoctorId = newDepartment.HeadDoctorId,
                    HeadDoctorName = $"{headDoctor?.FirstName} {headDoctor?.LastName}",
                    WardCount = 0
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating new department");
                throw;
            }
        }

        public async Task<DepartmentDto> UpdateDepartmentAsync(int id, DepartmentDto departmentDto)
        {
            try
            {
                ValidateDepartmentDto(departmentDto);

                if (id != departmentDto.Id)
                {
                    throw new ArgumentException("ID mismatch between route and DTO");
                }

                var existingDepartment = await _context.Departments.FindAsync(id);
                if (existingDepartment == null)
                {
                    throw new KeyNotFoundException($"Department with ID {id} not found");
                }

                var headDoctor = await _context.StaffProfiles.FindAsync(departmentDto.HeadDoctorId);
                if (departmentDto.HeadDoctorId.HasValue && headDoctor == null)
                {
                    throw new InvalidOperationException($"Head doctor with ID {departmentDto.HeadDoctorId} not found");
                }

                if (await DepartmentNameExists(departmentDto.Name, id))
                {
                    throw new InvalidOperationException($"Department with name '{departmentDto.Name}' already exists");
                }

                existingDepartment.Name = departmentDto.Name;
                existingDepartment.Description = departmentDto.Description;
                existingDepartment.HeadDoctorId = departmentDto.HeadDoctorId;

                await _context.SaveChangesAsync();

                return new DepartmentDto
                {
                    Id = existingDepartment.Id,
                    Name = existingDepartment.Name,
                    Description = existingDepartment.Description,
                    HeadDoctorId = existingDepartment.HeadDoctorId,
                    HeadDoctorName = $"{headDoctor?.FirstName} {headDoctor?.LastName}",
                    WardCount = existingDepartment.Wards?.Count ?? 0
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating department with ID {id}");
                throw;
            }
        }

        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            try
            {
                var department = await _context.Departments
                    .Include(d => d.Wards)
                    .FirstOrDefaultAsync(d => d.Id == id);

                if (department == null)
                {
                    throw new KeyNotFoundException($"Department with ID {id} not found");
                }

                if (department.Wards != null && department.Wards.Any())
                {
                    throw new InvalidOperationException("Cannot delete department that has wards assigned");
                }

                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting department with ID {id}");
                throw;
            }
        }

        #region Private Methods

        private static void ValidateDepartmentDto(DepartmentDto departmentDto)
        {
            if (departmentDto == null)
            {
                throw new ArgumentNullException(nameof(departmentDto));
            }

            if (string.IsNullOrWhiteSpace(departmentDto.Name))
            {
                throw new ArgumentException("Department name is required", nameof(departmentDto.Name));
            }

            if (departmentDto.HeadDoctorId <= 0)
            {
                throw new ArgumentException("Head doctor ID must be positive", nameof(departmentDto.HeadDoctorId));
            }
        }

        private async Task<bool> DepartmentNameExists(string name, int? excludeDepartmentId = null)
        {
            var query = _context.Departments
                .Where(d => d.Name == name);

            if (excludeDepartmentId.HasValue)
            {
                query = query.Where(d => d.Id != excludeDepartmentId.Value);
            }

            return await query.AnyAsync();
        }

        #endregion
    }
}
