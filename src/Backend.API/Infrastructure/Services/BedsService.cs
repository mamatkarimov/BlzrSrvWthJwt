using Backend.API.Application.DTOs;
using Backend.API.Application.Interfaces;
using Backend.API.Data;
using Backend.API.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.API.Infrastructure.Services
{
    public class BedService : IBedService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BedService> _logger;

        public BedService(ApplicationDbContext context, ILogger<BedService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<BedDto>> GetAllAsync()
        {
            try
            {
                // Load wards first to avoid N+1 queries
                var bedsWithWards = await _context.Beds
                    .AsNoTracking()
                    .Include(b => b.Ward)
                    .ToListAsync();

                return bedsWithWards.Select(b => new BedDto
                {
                    Id = b.Id,
                    BedNumber = b.BedNumber,
                    IsOccupied = b.IsOccupied,
                    WardId = b.WardId,
                    WardNumber = b.Ward?.WardNumber ?? "Unknown Ward"
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all beds");
                throw;
            }
        }

        public async Task<BedDto?> GetByIdAsync(int id)
        {
            try
            {
                var bed = await _context.Beds
                    .AsNoTracking()
                    .Include(b => b.Ward)
                    .FirstOrDefaultAsync(b => b.Id == id);

                return bed != null ? MapToDto(bed) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting bed with ID {id}");
                throw;
            }
        }

        public async Task<IEnumerable<BedDto>> GetBedsByWardAsync(int wardId)
        {
            try
            {
                var beds = await _context.Beds
                    .AsNoTracking()
                    .Where(b => b.WardId == wardId)
                    .Include(b => b.Ward)
                    .ToListAsync();

                return beds.Select(b => new BedDto
                {
                    Id = b.Id,
                    BedNumber = b.BedNumber,
                    IsOccupied = b.IsOccupied,
                    WardId = b.WardId,
                    WardNumber = b.Ward?.WardNumber ?? "Unknown Ward"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting beds for ward {wardId}");
                throw;
            }
        }

        public async Task<IEnumerable<BedDto>> GetAvailableBedsByWardAsync(int wardId)
        {
            try
            {
               var beds = await _context.Beds
                    .AsNoTracking()
                    .Where(b => b.WardId == wardId && !b.IsOccupied)
                    .Include(b => b.Ward)                   
                    .ToListAsync();

                return beds.Select(b => new BedDto
                {
                    Id = b.Id,
                    BedNumber = b.BedNumber,
                    IsOccupied = b.IsOccupied,
                    WardId = b.WardId,
                    WardNumber = b.Ward?.WardNumber ?? "Unknown Ward"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting available beds for ward {wardId}");
                throw;
            }
        }

        public async Task<BedDto> CreateBedAsync(BedDto bedDto)
        {
            try
            {
                ValidateBedDto(bedDto);

                var ward = await _context.Wards.FindAsync(bedDto.WardId)
                    ?? throw new InvalidOperationException($"Ward with ID {bedDto.WardId} does not exist.");

                if (await BedNumberExistsInWard(bedDto.WardId, bedDto.BedNumber))
                {
                    throw new InvalidOperationException($"Bed with number {bedDto.BedNumber} already exists in Ward {bedDto.WardId}.");
                }

                var newBed = new Bed
                {
                    BedNumber = bedDto.BedNumber,
                    IsOccupied = bedDto.IsOccupied,
                    WardId = bedDto.WardId
                };

                _context.Beds.Add(newBed);
                await _context.SaveChangesAsync();

                return MapToDto(newBed, ward.WardNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new bed");
                throw;
            }
        }

        public async Task<BedDto> UpdateBedAsync(int id, BedDto bedDto)
        {
            try
            {
                ValidateBedDto(bedDto);

                var existingBed = await _context.Beds.FindAsync(id)
                    ?? throw new KeyNotFoundException($"Bed with ID {id} not found.");

                var ward = await _context.Wards.FindAsync(bedDto.WardId)
                    ?? throw new InvalidOperationException($"Ward with ID {bedDto.WardId} does not exist.");

                if (await BedNumberExistsInWard(bedDto.WardId, bedDto.BedNumber, id))
                {
                    throw new InvalidOperationException($"Bed with number {bedDto.BedNumber} already exists in Ward {bedDto.WardId}.");
                }

                existingBed.BedNumber = bedDto.BedNumber;
                existingBed.IsOccupied = bedDto.IsOccupied;
                existingBed.WardId = bedDto.WardId;

                await _context.SaveChangesAsync();

                return MapToDto(existingBed, ward.WardNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating bed with ID {id}");
                throw;
            }
        }

        public async Task<BedDto> UpdateBedOccupiedStatusAsync(int id, bool isOccupied)
        {
            try
            {
                var bed = await _context.Beds
                    .Include(b => b.Ward)
                    .FirstOrDefaultAsync(b => b.Id == id)
                    ?? throw new KeyNotFoundException($"Bed with ID {id} not found.");

                bed.IsOccupied = isOccupied;
                await _context.SaveChangesAsync();

                return MapToDto(bed);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating occupied status for bed with ID {id}");
                throw;
            }
        }

        public async Task<bool> DeleteBedAsync(int id)
        {
            try
            {
                var bed = await _context.Beds.FindAsync(id)
                    ?? throw new KeyNotFoundException($"Bed with ID {id} not found.");

                _context.Beds.Remove(bed);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting bed with ID {id}");
                throw;
            }
        }

        #region Private Methods

        private static BedDto MapToDto(Bed bed, string? wardNumber = null)
        {
            return new BedDto
            {
                Id = bed.Id,
                BedNumber = bed.BedNumber,
                IsOccupied = bed.IsOccupied,
                WardId = bed.WardId,
                WardNumber = wardNumber ?? bed.Ward?.WardNumber ?? "Unknown Ward"
            };
        }

        private static void ValidateBedDto(BedDto bedDto)
        {
            if (bedDto == null)
            {
                throw new ArgumentNullException(nameof(bedDto));
            }

            if (string.IsNullOrWhiteSpace(bedDto.BedNumber))
            {
                throw new ArgumentException("Bed number must be provided.", nameof(bedDto.BedNumber));
            }

            if (bedDto.WardId <= 0)
            {
                throw new ArgumentException("Ward ID must be a positive number.", nameof(bedDto.WardId));
            }
        }

        private async Task<bool> BedNumberExistsInWard(int wardId, string bedNumber, int? excludeBedId = null)
        {
            var query = _context.Beds
                .Where(b => b.WardId == wardId && b.BedNumber == bedNumber);

            if (excludeBedId.HasValue)
            {
                query = query.Where(b => b.Id != excludeBedId.Value);
            }

            return await query.AnyAsync();
        }

        #endregion
    }
}
