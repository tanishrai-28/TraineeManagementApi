using Microsoft.AspNetCore.Mvc;
using TraineeManagementApi.Models;
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
        public IActionResult GetAll() {
            return Ok(_service.GetAll());
        }

        [HttpGet("{id}")] 
        public IActionResult GetTrainee(int id) {
            var trainee = _service.GetById(id);

            if(trainee == null) return NotFound();

            return Ok(trainee);
        }

        [HttpPost]
        public IActionResult Create(CreateTraineeRequest request) {
            var trainee =  _service.Create(request);

            return Created("/api/trainees",
                trainee
            );
        }

        [HttpPut("{id}")] 
        public IActionResult Update(long id, UpdateTraineeRequest request) {
            bool updated = _service.Update(id, request);

            if(!updated) {
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id) {
            bool deleted = _service.Delete(id);

            if(!deleted) {
                return NotFound();
            }

            return NoContent();
        }

    }
}