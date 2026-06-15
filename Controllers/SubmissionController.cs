using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagementApi.DTO.SubmissionDTO;
using TraineeManagementApi.Services.Interface;

namespace TraineeManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SubmissionController : ControllerBase
{
    public readonly ISubmissionService _service;
    private readonly ILogger<SubmissionController> _logger;

    public SubmissionController(ISubmissionService service, ILogger<SubmissionController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            return Ok(await _service.GetAllAsync());
        }
        catch (Exception e)
        {
            _logger.LogError($"Failed to fetch Submissions");
            return StatusCode(500, new
            {
                Message = "An unexpected error occured",
                Error = e.Message
            });
        }
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

        try
        {
            var submission = await _service.GetByIdAsync(id);

            if (submission == null)
            {
                _logger.LogInformation("Submission record not found");
                return NotFound();
            }

            return Ok(submission);
        }
        catch
        {
            _logger.LogError($"Failed to fetch submission with id {id}");
            return StatusCode(500, new
            {
                Message = "An unexpected error occured"
            });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateSubmissionRequest request)
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
            var submissions = await _service.CreateAsync(request);

            return Created("/api/submissions",
                submissions
            );
        }
        catch
        {
            _logger.LogError($"Failed to create submission");
            return StatusCode(500, new
            {
                Message = "An unexpected error occured"
            });
        }
    }
}