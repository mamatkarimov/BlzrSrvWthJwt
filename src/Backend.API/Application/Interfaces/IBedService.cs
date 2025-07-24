using Backend.API.Domain.Entities;
using Backend.API.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.API.Application.Interfaces
{
    public interface IBedService
    {
        Task<List<BedDto>> GetAllAsync();
        Task<BedDto?> GetByIdAsync(int id);
        Task<IEnumerable<BedDto>> GetBedsByWardAsync(int wardId);
        Task<IEnumerable<BedDto>> GetAvailableBedsByWardAsync(int wardId);
        Task<BedDto> CreateBedAsync(BedDto bedDto);
        Task<BedDto> UpdateBedAsync(int id, BedDto bedDto);
        Task<BedDto> UpdateBedOccupiedStatusAsync(int id, bool isOccupied);
        Task<bool> DeleteBedAsync(int id);
    }   
}