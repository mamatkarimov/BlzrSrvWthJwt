using Backend.API.Application.DTOs;
using Backend.API.Application.Interfaces;
using Backend.API.Infrastructure.Services;
using Backend.API.Models;
using Backend.API.Permissions;
using Backend.API.Settings;
using Backend.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSystem.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;
        private readonly ILogger<PatientController> _logger;

        public PatientController(IPatientService patientService, ILogger<PatientController> logger)
        {
            _patientService = patientService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientDto>>> GetAllPatients()
        {
            try
            {
                var employees = await _patientService.GetAllAsync();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all patients");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("register-patient")]
        [Authorize(Policy = AdministrativePermission.AdministrativeManageUser)]
        public async Task<ActionResult<BaseApiResponse<string>>> RegisterPatient([FromBody] PatientRegisterInput input)
        {
            if (!ModelState.IsValid)
            {
                var response = new BaseApiResponse<string>();
                response.AddModelErrors(ModelState);
                return BadRequest(response);
            }

            var result = await _patientService.CreateAsync(input);
            if (result.Succeeded)
                return Ok(new BaseApiResponse<string>("OK"));

            return BadRequest(new BaseApiResponse<string> { Errors = result.Errors });
        }
    }
}