using Backend.API.Application.DTOs;
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
    public class StaffController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public StaffController(IPatientService patientService)
        {
            _patientService = patientService;
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