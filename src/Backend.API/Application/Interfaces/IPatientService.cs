using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.API.Application.DTOs
{
    public interface IPatientService
    {
        Task<List<PatientDto>> GetAllAsync();
        Task<PatientDto> CreateAsync(PatientCreateDto input);
    }   
}