using TraineeManagementApi.DTO.SubmissionFileDTO;

namespace TraineeManagementApi.Services.FileStorage;

public interface ISubmissionFileService
{
    Task<SubmissionFileResponse> UploadAsync(long submissionId, IFormFile file, string uploadedBy, CancellationToken cancellationToken = default);
    Task<SubmissionFileDownloadDto> DownloadAsync (Guid id, CancellationToken cancellationToken);
    Task DeleteAsync (Guid id, CancellationToken cancellationToken);
}