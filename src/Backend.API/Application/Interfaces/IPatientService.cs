using Backend.API.Application.DTOs;
using Backend.API.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.API.Application.Interfaces
{
    public interface IPatientService
    {
        Task<List<PatientDto>> GetAllAsync();
        Task<BaseServiceResult<List<string>>> CreateAsync(PatientRegisterInput input);
        Task<PatientDto> GetByIdAsync(Guid id);
    }   
}