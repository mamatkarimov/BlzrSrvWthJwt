using Backend.API.Application.Interfaces;
using Backend.API.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicalSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BedController : ControllerBase
    {
        private readonly IBedService _bedService;
        private readonly ILogger<BedController> _logger;

        public BedController(IBedService bedService, ILogger<BedController> logger)
        {
            _bedService = bedService;
            _logger = logger;
        }

        // GET: api/Beds
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BedDto>>> GetAllBeds()
        {
            try
            {
                var beds = await _bedService.GetAllAsync();
                return Ok(beds);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all beds");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/Beds/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BedDto>> GetBed(int id)
        {
            try
            {
                var bed = await _bedService.GetByIdAsync(id);

                if (bed == null)
                {
                    return NotFound();
                }

                return Ok(bed);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting bed with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/Beds/ByWard/5
        [HttpGet("ByWard/{wardId}")]
        public async Task<ActionResult<IEnumerable<BedDto>>> GetBedsByWard(int wardId)
        {
            try
            {
                var beds = await _bedService.GetBedsByWardAsync(wardId);
                return Ok(beds);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting beds for ward {wardId}");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/Beds/Available/ByWard/5
        [HttpGet("Available/ByWard/{wardId}")]
        public async Task<ActionResult<IEnumerable<BedDto>>> GetAvailableBedsByWard(int wardId)
        {
            try
            {
                var beds = await _bedService.GetAvailableBedsByWardAsync(wardId);
                return Ok(beds);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting available beds for ward {wardId}");
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/Beds
        [HttpPost]
        public async Task<ActionResult<BedDto>> CreateBed(BedDto bedDto)
        {
            try
            {
                var createdBed = await _bedService.CreateBedAsync(bedDto);
                return CreatedAtAction(nameof(GetBed), new { id = createdBed.Id }, createdBed);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating new bed");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT: api/Beds/5
        [HttpPut("{id}")]
        public async Task<ActionResult<BedDto>> UpdateBed(int id, BedDto bedDto)
        {
            try
            {
                if (id != bedDto.Id)
                {
                    return BadRequest("ID mismatch");
                }

                var updatedBed = await _bedService.UpdateBedAsync(id, bedDto);
                return Ok(updatedBed);
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
                _logger.LogError(ex, $"Error updating bed with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        // PATCH: api/Beds/Occupied/5
        [HttpPatch("Occupied/{id}")]
        public async Task<ActionResult<BedDto>> UpdateBedOccupiedStatus(int id, [FromBody] bool isOccupied)
        {
            try
            {
                var updatedBed = await _bedService.UpdateBedOccupiedStatusAsync(id, isOccupied);
                return Ok(updatedBed);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating occupied status for bed with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: api/Beds/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBed(int id)
        {
            try
            {
                var result = await _bedService.DeleteBedAsync(id);
                if (!result)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting bed with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}