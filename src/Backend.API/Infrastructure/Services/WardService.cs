using Backend.API.Application.DTOs;
using Backend.API.Application.Interfaces;
using Backend.API.Data;
using Backend.API.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.API.Infrastructure.Services
{
    public class WardService : IWardService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<WardService> _logger;

        public WardService(ApplicationDbContext context, ILogger<WardService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<WardDto>> GetAllWardsAsync()
        {
            try
            {
                var wards = await _context.Wards
                    .AsNoTracking()
                    .Include(w => w.Department)
                    .Include(w => w.Beds)                    
                    .ToListAsync();

                return wards.Select(ward => new WardDto
                {
                    Id = ward.Id,
                    WardNumber = ward.WardNumber,
                    Capacity = ward.Capacity,
                    GenderSpecific = ward.GenderSpecific,
                    DepartmentId = ward.DepartmentId,
                    DepartmentName = ward.Department?.Name ?? "Unknown Department",
                    BedCount = ward.Beds?.Count ?? 0
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all wards");
                throw;
            }
        }

        public async Task<WardDto?> GetWardByIdAsync(int id)
        {
            try
            {
                var ward = await _context.Wards
                    .AsNoTracking()
                    .Include(w => w.Department)
                    .Include(w => w.Beds)
                    .FirstOrDefaultAsync(w => w.Id == id);

                return ward != null ? MapToDto(ward) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting ward with ID {id}");
                throw;
            }
        }

        public async Task<IEnumerable<WardDto>> GetWardsByDepartmentAsync(int departmentId)
        {
            try
            {
                var wards = await _context.Wards
                    .AsNoTracking()
                    .Where(w => w.DepartmentId == departmentId)
                    .Include(w => w.Beds)                    
                    .ToListAsync();

                return wards.Select(ward => new WardDto
                {
                    Id = ward.Id,
                    WardNumber = ward.WardNumber,
                    Capacity = ward.Capacity,
                    GenderSpecific = ward.GenderSpecific,
                    DepartmentId = ward.DepartmentId,
                    DepartmentName = ward.Department?.Name ?? "Unknown Department",
                    BedCount = ward.Beds?.Count ?? 0
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting wards for department {departmentId}");
                throw;
            }
        }

        public async Task<WardDto> CreateWardAsync(WardDto wardDto)
        {
            try
            {
                ValidateWardDto(wardDto);

                var department = await _context.Departments.FindAsync(wardDto.DepartmentId)
                    ?? throw new InvalidOperationException($"Department with ID {wardDto.DepartmentId} does not exist.");

                if (await WardNumberExistsInDepartment(wardDto.DepartmentId, wardDto.WardNumber))
                {
                    throw new InvalidOperationException($"Ward with number {wardDto.WardNumber} already exists in Department {wardDto.DepartmentId}.");
                }

                var newWard = new Ward
                {
                    WardNumber = wardDto.WardNumber,
                    Capacity = wardDto.Capacity,
                    GenderSpecific = wardDto.GenderSpecific,
                    DepartmentId = wardDto.DepartmentId
                };

                _context.Wards.Add(newWard);
                await _context.SaveChangesAsync();

                return MapToDto(newWard, department.Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new ward");
                throw;
            }
        }

        public async Task<WardDto> UpdateWardAsync(int id, WardDto wardDto)
        {
            try
            {
                ValidateWardDto(wardDto);

                var existingWard = await _context.Wards.FindAsync(id)
                    ?? throw new KeyNotFoundException($"Ward with ID {id} not found.");

                var department = await _context.Departments.FindAsync(wardDto.DepartmentId)
                    ?? throw new InvalidOperationException($"Department with ID {wardDto.DepartmentId} does not exist.");

                if (await WardNumberExistsInDepartment(wardDto.DepartmentId, wardDto.WardNumber, id))
                {
                    throw new InvalidOperationException($"Ward with number {wardDto.WardNumber} already exists in Department {wardDto.DepartmentId}.");
                }

                existingWard.WardNumber = wardDto.WardNumber;
                existingWard.Capacity = wardDto.Capacity;
                existingWard.GenderSpecific = wardDto.GenderSpecific;
                existingWard.DepartmentId = wardDto.DepartmentId;

                await _context.SaveChangesAsync();

                return MapToDto(existingWard, department.Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating ward with ID {id}");
                throw;
            }
        }

        public async Task<bool> DeleteWardAsync(int id)
        {
            try
            {
                var ward = await _context.Wards.FindAsync(id)
                    ?? throw new KeyNotFoundException($"Ward with ID {id} not found.");

                // Check if ward has beds
                if (await _context.Beds.AnyAsync(b => b.WardId == id))
                {
                    throw new InvalidOperationException("Cannot delete ward that has beds assigned.");
                }

                _context.Wards.Remove(ward);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting ward with ID {id}");
                throw;
            }
        }

        #region Private Methods

        private static WardDto MapToDto(Ward ward, string? departmentName = null)
        {
            return new WardDto
            {
                Id = ward.Id,
                WardNumber = ward.WardNumber,
                Capacity = ward.Capacity,
                GenderSpecific = ward.GenderSpecific,
                DepartmentId = ward.DepartmentId,
                DepartmentName = departmentName ?? ward.Department?.Name ?? "Unknown Department",
                BedCount = ward.Beds?.Count ?? 0
            };
        }

        private static void ValidateWardDto(WardDto wardDto)
        {
            if (wardDto == null)
            {
                throw new ArgumentNullException(nameof(wardDto));
            }

            if (string.IsNullOrWhiteSpace(wardDto.WardNumber))
            {
                throw new ArgumentException("Ward number must be provided.", nameof(wardDto.WardNumber));
            }

            if (wardDto.DepartmentId <= 0)
            {
                throw new ArgumentException("Department ID must be a positive number.", nameof(wardDto.DepartmentId));
            }

            if (wardDto.Capacity <= 0)
            {
                throw new ArgumentException("Capacity must be a positive number.", nameof(wardDto.Capacity));
            }
        }

        private async Task<bool> WardNumberExistsInDepartment(int departmentId, string wardNumber, int? excludeWardId = null)
        {
            var query = _context.Wards
                .Where(w => w.DepartmentId == departmentId && w.WardNumber == wardNumber);

            if (excludeWardId.HasValue)
            {
                query = query.Where(w => w.Id != excludeWardId.Value);
            }

            return await query.AnyAsync();
        }

        #endregion
    }
}
