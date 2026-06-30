using Microsoft.AspNetCore.Mvc;
using TraineeManagementApi.Services.Interface;
using TraineeManagementApi.DTO.UserDTO;

namespace TraineeManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _service;
    public AuthController(IUserService service)
    {
        _service = service;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginUser(UserLogin request)
    {
        LoginResponse isValid = await _service.LoginUser(request);

        return Ok(isValid);
    }
}
