using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagementApi.DTO.TaskAssignmentDTO;
using TraineeManagementApi.Services.Interface;

namespace TraineeManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TaskAssignmentController : ControllerBase
{
    public readonly ITaskAssignmentService _service;
    private readonly ILogger<TaskAssignmentController> _logger;

    public TaskAssignmentController(ITaskAssignmentService service, ILogger<TaskAssignmentController> logger)
    {
        _service = service;
        _logger = logger;
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
            _logger.LogInformation("Task Assignment record not found");
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
            _logger.LogInformation("Task assignment record not found");
            return NotFound();
        }

        return Ok();
    }

}
