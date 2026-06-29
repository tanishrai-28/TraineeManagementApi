using Microsoft.AspNetCore.Mvc;
using TraineeManagementApi.Services.RabbitMq;

namespace TraineeManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize]
public class ProcessingJobController : ControllerBase
{
    public readonly IProcessingJobService _service;

    public ProcessingJobController(IProcessingJobService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var job = await _service.GetByIdAsync(id);

        if (job == null)
        {
            return NotFound();
        }

        return Ok(job);
    }

    [HttpGet]
    public async Task<IActionResult> GetAl()
    {
        return Ok(await _service.GetAll());
    }

    [HttpGet("status/{id}")]
    public async Task<IActionResult> GetStatusById(Guid id)
    {
        return Ok(await _service.GetStatusById(id));
    }

    [HttpPost("{id}/retry")] 
    public async Task<IActionResult> RetryJob(Guid id)
    {
        var processingJob = await _service.RetryJob(id);
        if(processingJob == null)
        {
            return NotFound();
        }

        return Ok(processingJob);
    }
}