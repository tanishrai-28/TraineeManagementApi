using Microsoft.EntityFrameworkCore;
using TraineeManagementApi.Context;
using TraineeManagementApi.DTO.ProcessingJobDTO;

namespace TraineeManagementApi.Services.RabbitMq;

public class ProcessingJobService : IProcessingJobService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ProcessingJobService> _logger;
    public ProcessingJobService(ApplicationDbContext context, ILogger<ProcessingJobService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ProcessingJobResponse?>  GetByIdAsync(Guid id)
    {
        return await _context.ProcessingJobs
            .Where(x => x.Id == id)
            .Select(x => new  ProcessingJobResponse
            {
                Id = x.Id,
                MessageId = x.MessageId,
                CorrelationId = x.CorelationId,
                Attempts = x.Attempts,
                Status = x.Status,
                ErrorSummary = x.ErrorSummary,
                StartedTime = x.StartedTime,
                CompletedTime = x.CompletedTime,
            })
            .FirstOrDefaultAsync();
    }
}