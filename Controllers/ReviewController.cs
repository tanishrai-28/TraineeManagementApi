using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagementApi.DTO.ReviewDTO;
using TraineeManagementApi.Services.Interface;

namespace TraineeManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReviewController : ControllerBase
{
    public readonly IReviewService _service;

    public ReviewController(IReviewService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetReview(long id)
    {
        if (id <= 0)
        {
            return BadRequest(new
            {
                Message = "id must be greater than equal to 0"
            });
        }

        var review = await _service.GetByIdAsync(id);

        if (review == null)
        {
            return NotFound();
        }

        return Ok(review);

    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateReviewRequest request)
    {
        if (request == null)
        {
            return BadRequest(new
            {
                Message = "Request body cannot be empty"
            });
        }

        var reviews = await _service.CreateAsync(request);

        return Created("/api/reviews",
            reviews
        );
    }
}