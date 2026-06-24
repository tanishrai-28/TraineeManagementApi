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

    public LearningTaskController(ILearningTaskService service)
    {
        _service = service;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllAsync());
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

        var learningTask = await _service.GetByIdAsync(id);

        if (learningTask == null)
        {
            return NotFound();
        }

        return Ok(learningTask);
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

        var learningTask = await _service.CreateAsync(request);

        return Created("/api/learningTasks",
            learningTask
        );

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

        bool updated = await _service.UpdateAsync(id, request);

        if (!updated)
        {
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
            return NotFound();
        }

        return NoContent();

    }
}