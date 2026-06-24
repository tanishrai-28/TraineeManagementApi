using Microsoft.AspNetCore.Mvc;
using TraineeManagementApi.DTO.SubmissionDTO;
using TraineeManagementApi.Services.FileStorage;
using TraineeManagementApi.Services.Interface;

namespace TraineeManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize]
public class SubmissionController : ControllerBase
{
    public readonly ISubmissionService _service;
    public readonly ISubmissionFileService _submissionFileService;

    public SubmissionController(ISubmissionService service, ISubmissionFileService submissionFileService)
    {
        _service = service;
        _submissionFileService = submissionFileService;
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

    [HttpPost("{submissionId}/files")]
    public async Task<IActionResult> UploadFile (long submissionId, IFormFile file, CancellationToken cancellationToken)
    {
        var uploadedBy = User.Identity?.Name ?? "System";

        var response = await _submissionFileService.UploadAsync(submissionId, file, uploadedBy, cancellationToken);

        return CreatedAtAction(
            nameof(GetSubmission),
            new {id = response.Id},
            response
        );
    }

    [HttpGet("files/{id}/download")]
    public async Task<IActionResult> DownloadFile(Guid id, CancellationToken cancellationToken)
    {
        var file = await _submissionFileService.DownloadAsync(id, cancellationToken);

        return File(
            file.Stream,
            file.ContentType,
            file.FileName
        );
    }

    [HttpDelete("files/{id}")]
    public async Task<IActionResult> DeleteFile(Guid id, CancellationToken cancellationToken)
    {
        await _submissionFileService.DeleteAsync(id, cancellationToken);

        return NoContent();
    }
}