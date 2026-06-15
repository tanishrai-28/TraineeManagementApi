using TraineeManagementApi.DTO.TaskAssignmentDTO;

namespace TraineeManagementApi.Services.Interface;

public interface ITaskAssignmentService
{
    Task<List<TaskAssignmentResponse>> GetAllAsync();
    
    Task<TaskAssignmentResponse?> GetByIdAsync(long id);
 
    Task<TaskAssignmentResponse> CreateAsync(CreateTaskAssignmentRequest request);
    
    Task<bool> UpdateStatusAsync(long id, UpdateTaskAssignmentStatusRequest request);
}