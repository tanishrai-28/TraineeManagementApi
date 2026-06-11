using Microsoft.AspNetCore.Authorization;
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
        private readonly ILogger<TraineesController> _logger;


        public TraineesController(ITraineeService service, ILogger<TraineesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] TraineeQueryFilter filter, CancellationToken cancellationToken = default)
        {
            try
            {
                return Ok(await _service.GetAllAsync(filter, cancellationToken));
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to fetch trainees");
                return StatusCode(500, new
                {
                    Message = "An unexpected error occured",
                    Error = e.Message
                });
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetTrainee(long id)
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
                var trainee = await _service.GetByIdAsync(id);

                if (trainee == null)
                {
                    _logger.LogInformation("User record not found");
                    return NotFound();
                }

                return Ok(trainee);
            }
            catch
            {
                _logger.LogError($"Failed to fetch user with id {id}");
                return StatusCode(500, new
                {
                    Message = "An unexpected error occured"
                });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateTraineeRequest request)
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
                var trainee = await _service.CreateAsync(request);

                return Created("/api/trainees",
                    trainee
                );
            }
            catch
            {
                _logger.LogError($"Failed to create user");
                return StatusCode(500, new
                {
                    Message = "An unexpected error occured"
                });
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(long id, UpdateTraineeRequest request)
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
                bool updated = await _service.UpdateAsync(id, request);

                if (!updated)
                {
                    _logger.LogInformation("User record not found");
                    return NotFound();
                }

                return Ok();
            }
            catch
            {
                _logger.LogError($"Failed to update user with id {id}");
                return StatusCode(500, new
                {
                    Message = "An unexpected error occured"
                });
            }
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

            try
            {
                bool deleted = await _service.DeleteAsync(id);

                if (!deleted)
                {
                    _logger.LogError($"User to be deleted not found");
                    return NotFound();
                }

                return NoContent();
            }
            catch
            {
                _logger.LogError($"Failed to delete user with id {id}");
                return StatusCode(500, new
                {
                    Message = "An unexpected error occured"
                });
            }
        }

    }
}