using TraineeManagementApi.DTO.LearningTaskDTO;

namespace TraineeManagementApi.Services.Interface;

public interface ILearningTaskService
{
    Task<List<LearningTaskResponse>> GetAllAsync();
    
    Task<LearningTaskResponse?> GetByIdAsync(long id);

    Task<LearningTaskResponse> CreateAsync(CreateLearningTaskRequest request);

    Task<bool> UpdateAsync(long id, UpdateLearningTaskRequest request);

    Task<bool> DeleteAsync(long id);
}