using TraineeManagementApi.DTO.MentorDTO;

namespace TraineeManagementApi.Services.Interface;

public interface IMentorService
{
    Task<List<MentorResponse>> GetAllAsync();
    Task<MentorResponse?> GetByIdAsync(long id);

    Task<MentorResponse> CreateAsync(CreateMentorRequest request);

    Task<bool> UpdateAsync(long id, UpdateMentorRequest request);

    Task<bool> DeleteAsync(long id);
}