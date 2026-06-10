using TraineeManagementApi.DTO;

namespace TraineeManagementApi.Services {

    public interface IUserService{
        Task<LoginResponse> LoginUser(UserLogin req);
    }

}