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

        if(job == null)
        {
            return NotFound();
        }

        return Ok(job);
    }
}