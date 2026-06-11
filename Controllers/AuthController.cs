using Microsoft.AspNetCore.Mvc;
using TraineeManagementApi.DTO;
using TraineeManagementApi.Services;

namespace TraineeManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IUserService _service;
    public AuthController(IUserService service, ILogger<AuthController> logger)
    {
        _logger = logger;
        _service = service;
    }


    [HttpPost("login")]
    public async Task<IActionResult> LoginUser(UserLogin request)
    {
        try
        {
            LoginResponse isValid = await _service.LoginUser(request);

            return Ok(isValid);
        }
        catch
        {
            _logger.LogError("Failed to login user");
            return StatusCode(500, new
            {
                Message = "An unexpected error occured"
            });

        }
    }
}
