using Backend.API.Application.DTOs;
using Backend.API.Application.Interfaces;
using Backend.API.Domain.Entities;
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
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _staffService;
        private readonly ILogger<StaffController> _logger;

        public StaffController(IStaffService staffService, ILogger<StaffController> logger)
        {
            _staffService = staffService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StaffDto>>> GetAllEmployees()
        {
            try
            {
                var employees = await _staffService.GetAllAsync();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all employees");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("register-employee")]
        [Authorize(Policy = AdministrativePermission.AdministrativeManageUser)]
        public async Task<ActionResult<BaseApiResponse<string>>> RegisterEmployee([FromBody] StaffRegisterInput input)
        {
            if (!ModelState.IsValid)
            {
                var response = new BaseApiResponse<string>();
                response.AddModelErrors(ModelState);
                return BadRequest(response);
            }

            var result = await _staffService.CreateAsync(input);
            if (result.Succeeded)
                return Ok(new BaseApiResponse<string>("OK"));

            return BadRequest(new BaseApiResponse<string> { Errors = result.Errors });
        }
    }
}