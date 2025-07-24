using Backend.API.Application.DTOs;
using Backend.API.Application.Interfaces;
using Backend.API.Data;
using Backend.API.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicalSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WardController : ControllerBase
    {
        private readonly IWardService _wardService;
        private readonly ILogger<WardController> _logger;

        public WardController(IWardService wardService, ILogger<WardController> logger)
        {
            _wardService = wardService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WardDto>>> GetAllWards()
        {
            try
            {
                var wards = await _wardService.GetAllWardsAsync();
                return Ok(wards);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all wards");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WardDto>> GetWard(int id)
        {
            try
            {
                var ward = await _wardService.GetWardByIdAsync(id);

                if (ward == null)
                {
                    return NotFound();
                }

                return Ok(ward);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting ward with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("ByDepartment/{departmentId}")]
        public async Task<ActionResult<IEnumerable<WardDto>>> GetWardsByDepartment(int departmentId)
        {
            try
            {
                var wards = await _wardService.GetWardsByDepartmentAsync(departmentId);
                return Ok(wards);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting wards for department {departmentId}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<WardDto>> CreateWard(WardDto wardDto)
        {
            try
            {
                var createdWard = await _wardService.CreateWardAsync(wardDto);
                return CreatedAtAction(nameof(GetWard), new { id = createdWard.Id }, createdWard);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating ward");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWard(int id, WardDto wardDto)
        {
            try
            {
                if (id != wardDto.Id)
                {
                    return BadRequest("ID mismatch");
                }

                var updatedWard = await _wardService.UpdateWardAsync(id, wardDto);
                return Ok(updatedWard);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating ward with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWard(int id)
        {
            try
            {
                var result = await _wardService.DeleteWardAsync(id);
                if (!result)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting ward with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}