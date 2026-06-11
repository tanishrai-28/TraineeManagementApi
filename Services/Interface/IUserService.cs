using TraineeManagementApi.DTO.UserDTO;

namespace TraineeManagementApi.Services.Interface;

public interface IUserService
{
    Task<LoginResponse> LoginUser(UserLogin req);
}