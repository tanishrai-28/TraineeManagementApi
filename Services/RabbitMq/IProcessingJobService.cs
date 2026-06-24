using TraineeManagementApi.DTO.ProcessingJobDTO;

namespace TraineeManagementApi.Services.RabbitMq;

public interface IProcessingJobService
{
    Task<ProcessingJobResponse?> GetByIdAsync(Guid id);
}