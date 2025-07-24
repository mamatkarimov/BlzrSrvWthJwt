using Backend.API.Domain.Entities;
using Backend.API.Models;

namespace Backend.API.Application.Interfaces
{
    public interface IWardService
    {
        Task<IEnumerable<WardDto>> GetAllWardsAsync();
        Task<WardDto?> GetWardByIdAsync(int id);
        Task<IEnumerable<WardDto>> GetWardsByDepartmentAsync(int departmentId);
        Task<WardDto> CreateWardAsync(WardDto wardDto);
        Task<WardDto> UpdateWardAsync(int id, WardDto wardDto);
        Task<bool> DeleteWardAsync(int id);
    }
}