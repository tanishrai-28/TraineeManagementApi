using TraineeManagementApi.DTO.ReviewDTO;

namespace TraineeManagementApi.Services.Interface;

public interface IReviewService
{
    Task<List<ReviewResponse>> GetAllAsync();
    
    Task<ReviewResponse?> GetByIdAsync(long id);
 
    Task<ReviewResponse> CreateAsync(CreateReviewRequest request);
}