using Backend.API.Application.DTOs;
using Backend.API.Application.Interfaces;
using Backend.API.Infrastructure.Services;
using Backend.API.Models;
using Backend.API.Permissions;
using Backend.API.Settings;
using Backend.Domain.Enums;
using BSMed.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [Authorize(Roles = "Receptionist,Administrator,Admin")]
        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<PatientRegistrationDto>>> RegisterPatient(
    [FromBody] StaffPatientRegisterInput input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<PatientRegistrationDto>.FromModelState(ModelState));

            var staffUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _patientService.RegisterPatientAsync(input, staffUserId);

            if (!result.IsSuccess)
                return BadRequest(ApiResponse<PatientRegistrationDto>.FromErrors(result.Errors));

            return Ok(new ApiResponse<PatientRegistrationDto>(new PatientRegistrationDto
            {
                PatientId = result.PatientId,
                Username = result.Username,
                TemporaryPassword = result.TemporaryPassword,
                RegistrationNote = input.RegistrationNote
            }));
        }


        [HttpPost("self-register-patient")]
        [AllowAnonymous] // Changed from Authorize to allow registration
        public async Task<ActionResult<BaseApiResponse<AuthResponse>>> SelfRegisterPatient(
    [FromBody] PatientRegisterInput input)
        {
            if (!ModelState.IsValid)
            {
                var response = new BaseApiResponse<AuthResponse>();
                response.AddModelErrors(ModelState);
                return BadRequest(response);
            }

            var result = await _patientService.CreateAsync(input);

            if (!result.Succeeded)
            {
                return BadRequest(new BaseApiResponse<AuthResponse>
                {
                    Errors = result.Errors
                });
            }

            // Optionally: Automatically log in the user after registration
            // var authResult = await _authService.AuthenticateAsync(input.UserName, input.Password);

            return Ok(new BaseApiResponse<AuthResponse>(new AuthResponse
            {
                Success = result.Succeeded
            }));
        }
    }
}