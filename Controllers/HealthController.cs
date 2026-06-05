using Microsoft.AspNetCore.Mvc;

namespace TraineeManagementApi.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase {
        [HttpGet]
        public object Get() {
            return new {
                status = "running",
                application = "Trainee Management Api",
                timestamp = DateTime.UtcNow
            };
        }
    }
}