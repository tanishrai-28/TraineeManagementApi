using TraineeManagementApi.DTO.SubmissionDTO;

namespace TraineeManagementApi.Services.Interface;

public interface ISubmissionService
{
    Task<List<SubmissionResponse>> GetAllAsync();
    
    Task<SubmissionResponse?> GetByIdAsync(long id);
 
    Task<SubmissionResponse> CreateAsync(CreateSubmissionRequest request);
}