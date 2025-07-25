using Backend.API.Application.DTOs;
using Backend.API.Models;
using BSMed.Shared;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.API.Application.Interfaces
{
    public interface IPatientService
    {
        Task<List<PatientDto>> GetAllAsync();
        Task<BaseServiceResult<List<string>>> CreateAsync(PatientRegisterInput input);        
        Task<PatientDto> GetByIdAsync(Guid id);
        Task<PatientRegistrationResult> RegisterPatientAsync(PatientRegisterInput input, string createdByStaffId);
        Task<IdentityResult> SelfCreateAsync(PatientRegisterInput input);
    }   
}