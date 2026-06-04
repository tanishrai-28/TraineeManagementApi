using Microsoft.AspNetCore.Mvc;

namespace TrainingManagementApi.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase {
        [HttpGet]
        public IActionResult Get() {
            return Ok(new
                {
                    status = "running",
                    application = "Trainee Management Api",
                    timestamp = DateTime.UtcNow
                }
            );
        }
    }
}