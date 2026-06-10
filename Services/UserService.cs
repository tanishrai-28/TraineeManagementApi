using TraineeManagementApi.DTO;
using TraineeManagementApi.Models;
using TraineeManagementApi.Context;
using TraineeManagementApi.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace TraineeManagementApi.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly int _expiresIn;

    public UserService(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
        _expiresIn = Convert.ToInt32(_configuration["JWT:ExpiresIn"]);
    }

    public async Task<LoginResponse> LoginUser(UserLogin request)
    {
        User? user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid Credentials!");
        }

        bool isValid = PasswordHasher.VerifyPassword(request.Password, user.PasswordHash);
        if (!isValid)
        {
            throw new UnauthorizedAccessException("Invalid Credentials!");
        }

        string token = GenerateToken(user.Id, user.Username, user.Role.ToString());

        LoginResponse res = new()
        {
            Token = token,
            ExpiresIn = _expiresIn,
            User = new
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role
            }
        };

        return res;


    }

    private string GenerateToken(long Id, string username, string role)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, Id.ToString()),
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
            claims: claims,
            expires: DateTime.Now.AddSeconds(Convert.ToInt32(_expiresIn)),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}