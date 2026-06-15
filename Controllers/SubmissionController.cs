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

        return Ok(await _service.GetAllAsync());

    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSubmission(long id)
    {
        if (id <= 0)
        {
            return BadRequest(new
            {
                Message = "id must be greater than equal to 0"
            });
        }


        var submission = await _service.GetByIdAsync(id);

        if (submission == null)
        {
            _logger.LogInformation("Submission record not found");
            return NotFound();
        }

        return Ok(submission);

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

        var submissions = await _service.CreateAsync(request);

        return Created("/api/submissions",
            submissions
        );


    }
}