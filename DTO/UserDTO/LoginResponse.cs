namespace TraineeManagementApi.DTO.UserDTO;

public class LoginResponse
{
    public required string Token { get; set; }

    public required int ExpiresIn { get; set; }

    public required object User {get; set;}
}