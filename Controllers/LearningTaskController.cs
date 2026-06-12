using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagementApi.DTO.LearningTaskDTO;
using TraineeManagementApi.Services.Interface;

namespace TraineeManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LearningTaskController : ControllerBase
{
    public readonly ILearningTaskService _service;
    private readonly ILogger<LearningTaskController> _logger;

    public LearningTaskController(ILearningTaskService service, ILogger<LearningTaskController> logger)
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
            _logger.LogError($"Failed to fetch Learning tasks");
            return StatusCode(500, new
            {
                Message = "An unexpected error occured",
                Error = e.Message
            });
        }
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetLearningTask(long id)
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
            var learningTask = await _service.GetByIdAsync(id);

            if (learningTask == null)
            {
                _logger.LogInformation("Learning task record not found");
                return NotFound();
            }

            return Ok(learningTask);
        }
        catch
        {
            _logger.LogError($"Failed to fetch learning task with id {id}");
            return StatusCode(500, new
            {
                Message = "An unexpected error occured"
            });
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateLearningTaskRequest request)
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
            var learningTask = await _service.CreateAsync(request);

            return Created("/api/learningTasks",
                learningTask
            );
        }
        catch
        {
            _logger.LogError($"Failed to create learning task");
            return StatusCode(500, new
            {
                Message = "An unexpected error occured"
            });
        }
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(long id, UpdateLearningTaskRequest request)
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
                _logger.LogInformation("Learning task record not found");
                return NotFound();
            }

            return Ok();
        }
        catch
        {
            _logger.LogError($"Failed to update learning task with id {id}");
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
                _logger.LogError($"Learning task to be deleted not found");
                return NotFound();
            }

            return NoContent();
        }
        catch
        {
            _logger.LogError($"Failed to delete learning task with id {id}");
            return StatusCode(500, new
            {
                Message = "An unexpected error occured"
            });
        }
    }
}