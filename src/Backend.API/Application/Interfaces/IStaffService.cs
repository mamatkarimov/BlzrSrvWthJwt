using Backend.API.Application.DTOs;
using Backend.API.Models;
using BSMed.Shared;
using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.API.Application.Interfaces
{
    public interface IStaffService
    {
        Task<List<StaffDto>> GetAllAsync();
        Task<BaseServiceResult<List<string>>> CreateAsync(StaffRegisterInput input);
        Task<StaffDto> GetByIdAsync(int id);
    }   
}