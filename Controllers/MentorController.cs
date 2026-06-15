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

        return Ok(await _service.GetAllAsync());

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


        var mentor = await _service.GetByIdAsync(id);

        if (mentor == null)
        {
            _logger.LogInformation("Mentor record not found");
            return NotFound();
        }

        return Ok(mentor);

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

        var mentor = await _service.CreateAsync(request);

        return Created("/api/mentors",
            mentor
        );

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

        bool updated = await _service.UpdateAsync(id, request);

        if (!updated)
        {
            _logger.LogInformation("Mentor record not found");
            return NotFound();
        }

        return Ok();

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


        bool deleted = await _service.DeleteAsync(id);

        if (!deleted)
        {
            _logger.LogError($"Mentor to be deleted not found");
            return NotFound();
        }

        return NoContent();

    }

}