using TraineeManagementApi.DTO.ProcessingJobDTO;
using TraineeManagementApi.Models;

namespace TraineeManagementApi.Services.RabbitMq;

public interface IProcessingJobService
{
    Task<ProcessingJobResponse?> GetByIdAsync(Guid id);
    Task<List<ProcessingJob>> GetAll();
    Task<string?> GetStatusById(Guid id);
    Task<ProcessingJob?> RetryJob(Guid processingJobId);
}