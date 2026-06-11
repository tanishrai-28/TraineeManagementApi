using TraineeManagementApi.DTO;
using TraineeManagementApi.DTO.Pagination;

namespace TraineeManagementApi.Services {

    public interface ITraineeService{
        Task<List<TraineeResponse>> GetAllAsync(TraineeQueryFilter filter, CancellationToken cancellationToken = default);
        // Task<List<TraineeResponse>> GetAllAsync(string search, TraineeQueryFilter filter, CancellationToken cancellationToken = default);

        Task<TraineeResponse?> GetByIdAsync(long id);

        Task<TraineeResponse> CreateAsync(CreateTraineeRequest request);

        Task<bool> UpdateAsync(long id, UpdateTraineeRequest request);

        Task<bool> DeleteAsync(long id);
    }

}