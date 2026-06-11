using System.ComponentModel.DataAnnotations;

namespace TraineeManagementApi.DTO.UserDTO;

public class UserLogin
{
    [Required(ErrorMessage = "Username is required.")]
    [MaxLength(50, ErrorMessage = "Username length can't be more than 50.")]
    [MinLength(5, ErrorMessage = "Username length should be min 5.")]
    public required string Username { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    public required string Password { get; set; }
}