using Microsoft.AspNetCore.Mvc;
using TraineeManagementApi.DTO;
using TraineeManagementApi.Services;

namespace TranineeManagementApi.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class TraineesController: ControllerBase {
        public readonly ITraineeService _service; 

        public TraineesController(ITraineeService service) {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string search = "") {
            return Ok(await _service.GetAllAsync(search));
        }

        [HttpGet("{id}")] 
        public async Task<IActionResult> GetTrainee(long id) {
            var trainee = await _service.GetByIdAsync(id);

            if(trainee == null) return NotFound();

            return Ok(trainee);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTraineeRequest request) {
            var trainee = await _service.CreateAsync(request);

            return Created("/api/trainees",
                trainee
            );
        }

        [HttpPut("{id}")] 
        public async Task<IActionResult> Update(long id, UpdateTraineeRequest request) {
            bool updated = await _service.UpdateAsync(id, request);

            if(!updated) {
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id) {
            bool deleted = await _service.DeleteAsync(id);

            if(!deleted) {
                return NotFound();
            }

            return NoContent();
        }

    }
}