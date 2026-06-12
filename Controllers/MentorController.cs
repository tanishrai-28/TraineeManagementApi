using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagementApi.DTO.MentorDTO;
using TraineeManagementApi.Services.Interface;

namespace TraineeManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MentorController : ControllerBase
{
    public readonly IMentorService _service;
    private readonly ILogger<MentorController> _logger;

    public MentorController(IMentorService service, ILogger<MentorController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            return Ok(await _service.GetAllAsync());
        }
        catch (Exception e)
        {
            _logger.LogError($"Failed to fetch mentors");
            return StatusCode(500, new
            {
                Message = "An unexpected error occured",
                Error = e.Message
            });
        }
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetMentor(long id)
    {
        if (id <= 0)
        {
            return BadRequest(new
            {
                Message = "id must be greater than equal to 0"
            });
        }

        try
        {
            var mentor = await _service.GetByIdAsync(id);

            if (mentor == null)
            {
                _logger.LogInformation("Mentor record not found");
                return NotFound();
            }

            return Ok(mentor);
        }
        catch
        {
            _logger.LogError($"Failed to fetch mentor with id {id}");
            return StatusCode(500, new
            {
                Message = "An unexpected error occured"
            });
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateMentorRequest request)
    {
        if (request == null)
        {
            return BadRequest(new
            {
                Message = "Request body cannot be empty"
            });
        }
        try
        {
            var mentor = await _service.CreateAsync(request);

            return Created("/api/mentors",
                mentor
            );
        }
        catch
        {
            _logger.LogError($"Failed to create mentor");
            return StatusCode(500, new
            {
                Message = "An unexpected error occured"
            });
        }
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(long id, UpdateMentorRequest request)
    {
        if (request == null)
        {
            return BadRequest(new
            {
                Message = "Request body cannot be empty"
            });
        }

        try
        {
            bool updated = await _service.UpdateAsync(id, request);

            if (!updated)
            {
                _logger.LogInformation("Mentor record not found");
                return NotFound();
            }

            return Ok();
        }
        catch
        {
            _logger.LogError($"Failed to update mentor with id {id}");
            return StatusCode(500, new
            {
                Message = "An unexpected error occured"
            });
        }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(long id)
    {
        if (id <= 0)
        {
            return BadRequest(new
            {
                Message = "id must be greater than equal to 0"
            });
        }

        try
        {
            bool deleted = await _service.DeleteAsync(id);

            if (!deleted)
            {
                _logger.LogError($"Mentor to be deleted not found");
                return NotFound();
            }

            return NoContent();
        }
        catch
        {
            _logger.LogError($"Failed to delete mentor with id {id}");
            return StatusCode(500, new
            {
                Message = "An unexpected error occured"
            });
        }
    }

}