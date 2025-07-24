using Backend.API.Domain.Entities;
using Backend.API.Models;

namespace Backend.API.Application.Interfaces
{
    public interface IDepartmentService
    {
        Task<List<DepartmentDto>> GetAllDepartmentsAsync();
        Task<DepartmentDto?> GetDepartmentByIdAsync(int id);
        Task<DepartmentDto> CreateDepartmentAsync(DepartmentDto departmentDto);
        Task<DepartmentDto> UpdateDepartmentAsync(int id, DepartmentDto departmentDto);
        Task<bool> DeleteDepartmentAsync(int id);
    }
}