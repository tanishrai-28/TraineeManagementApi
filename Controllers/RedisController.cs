using Microsoft.AspNetCore.Mvc;
using TraineeManagementApi.Services.Redis;

namespace TraineeManagementApi.Controllers {
    [ApiController]
    [Route("api/redis")]
    public class RedisController : ControllerBase {
        private readonly IRedisService _redisService;

        public RedisController(IRedisService redisService)
        {
            _redisService = redisService;
        }

        [HttpGet("health")]
        public async Task<IActionResult> CheckConnection()
        {
            var connected = await _redisService.IsConnectionAsync();

            if(!connected)
            {
                return StatusCode(503, new
                {
                    Message = "Redis unavailable"
                });
            }

            return Ok(new
            {
                Message = "Redis connected"
            });
        }
    }
}