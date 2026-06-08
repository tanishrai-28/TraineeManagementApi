using TraineeManagementApi.DTO;

namespace TraineeManagementApi.Services {

    public interface ITraineeService{
        Task<List<TraineeResponse>> GetAllAsync(string search);

        Task<TraineeResponse?> GetByIdAsync(long id);

        Task<TraineeResponse> CreateAsync(CreateTraineeRequest request);

        Task<bool> UpdateAsync(long id, UpdateTraineeRequest request);

        Task<bool> DeleteAsync(long id);
    }

}