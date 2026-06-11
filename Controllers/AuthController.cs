using Microsoft.AspNetCore.Mvc;
using TraineeManagementApi.Context;
using TraineeManagementApi.DTO;
using TraineeManagementApi.Models;
using TraineeManagementApi.Helpers;
using Microsoft.EntityFrameworkCore;
using TraineeManagementApi.Services;

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
        try
        {
            LoginResponse isValid = await _service.LoginUser(request);

            return Ok(isValid);
        }
        catch
        {
            return StatusCode(500, new
            {
                Message = "An unexpected error occured"
            });

        }
    }
}
