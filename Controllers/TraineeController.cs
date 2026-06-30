using Microsoft.AspNetCore.Mvc;
using TraineeManagementApi.DTO.TraineeDTO;
using TraineeManagementApi.DTO.Pagination;
using TraineeManagementApi.Services.Interface;

namespace TraineeManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TraineesController : ControllerBase
    {
        public readonly ITraineeService _service;


        public TraineesController(ITraineeService service)
        {
            _service = service;
        }

        [HttpGet]
        // [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] TraineeQueryFilter filter, CancellationToken cancellationToken = default)
        {
            return Ok(await _service.GetAllAsync(filter, cancellationToken));
        }

        [HttpGet("{id}")]
        // [Authorize]
        public async Task<IActionResult> GetTrainee(long id)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    Message = "id must be greater than equal to 0"
                });
            }

            var trainee = await _service.GetByIdAsync(id);

            if (trainee == null)
            {
                return NotFound();
            }

            return Ok(trainee);

        }

        [HttpPost]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateTraineeRequest request)
        {
            if (request == null)
            {
                return BadRequest(new
                {
                    Message = "Request body cannot be empty"
                });
            }

            var trainee = await _service.CreateAsync(request);

            return Created("/api/trainees",
                trainee
            );
        }



        [HttpPut("{id}")]
        // [Authorize]
        public async Task<IActionResult> Update(long id, UpdateTraineeRequest request)
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
        // [Authorize]
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
}