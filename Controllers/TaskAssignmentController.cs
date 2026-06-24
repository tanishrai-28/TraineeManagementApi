using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagementApi.DTO.TaskAssignmentDTO;
using TraineeManagementApi.Services.Interface;

namespace TraineeManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize]
public class TaskAssignmentController : ControllerBase
{
    public readonly ITaskAssignmentService _service;

    public TaskAssignmentController(ITaskAssignmentService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTaskAssignment(long id)
    {
        if (id <= 0)
        {
            return BadRequest(new
            {
                Message = "id must be greater than equal to 0"
            });
        }


        var taskAssignment = await _service.GetByIdAsync(id);

        if (taskAssignment == null)
        {
            return NotFound();
        }

        return Ok(taskAssignment);

    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTaskAssignmentRequest request)
    {
        if (request == null)
        {
            return BadRequest(new
            {
                Message = "Request body cannot be empty"
            });
        }

        var taskAssignments = await _service.CreateAsync(request);

        return Created("/api/taskAssignments",
            taskAssignments
        );

    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, UpdateTaskAssignmentStatusRequest request)
    {
        if (request == null)
        {
            return BadRequest(new
            {
                Message = "Request body cannot be empty"
            });
        }

        bool updated = await _service.UpdateStatusAsync(id, request);

        if (!updated)
        {
            return NotFound();
        }

        return Ok();
    }

}
