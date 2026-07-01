using Microsoft.EntityFrameworkCore;
using TraineeManagementApi.Configurations;
using TraineeManagementApi.Context;
using TraineeManagementApi.DTO.ProcessingJobDTO;
using TraineeManagementApi.DTO.SubmissionDTO;
using TraineeManagementApi.Exceptions;
using TraineeManagementApi.Models;

namespace TraineeManagementApi.Services.RabbitMq;

public class ProcessingJobService : IProcessingJobService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ProcessingJobService> _logger;
    private readonly IRabbitMqPublisher _rabbitMqPublisher;

    public ProcessingJobService(ApplicationDbContext context, IRabbitMqPublisher rabbitMqPublisher, ILogger<ProcessingJobService> logger)
    {
        _context = context;
        _rabbitMqPublisher = rabbitMqPublisher;
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

    public async Task<List<ProcessingJob>> GetAll()
    {
        return await _context.ProcessingJobs.ToListAsync();
    }

    public async Task<string?> GetStatusById(Guid id)
    {
        var processingJob = await _context.ProcessingJobs.FindAsync(id);
        if(processingJob == null)
        {
            throw new NotFoundException("Processing job not found");
        }
        return processingJob.Status;
    }

    public async Task<ProcessingJob?> RetryJob(Guid id)
    {
        var processingJob = await _context.ProcessingJobs.FindAsync(id);
        if(processingJob == null)
        {
            _logger.LogWarning("Processing job not found");
            throw new NotFoundException("Processing job not found");
        }

        if(!processingJob.Status.Equals("Failed"))
        {
            _logger.LogWarning("Processing job not failed.");
            throw new BadRequestException("Processing job is not failed");
        }

        processingJob.Attempts = 0;
        processingJob.ErrorSummary = "";
        processingJob.Status = "Queued";

        SubmissionProcessingRequested message = new SubmissionProcessingRequested
        {
            MessageId = processingJob.MessageId,
            CorrelationId = processingJob.CorelationId,
            SubmissionId = processingJob.SubmissionId,
            FileId = processingJob.SubmissionFileId,
            RequestedAt = DateTime.UtcNow
        };

        await _context.SaveChangesAsync();
        await _rabbitMqPublisher.PublishAsync(message, RabbitMQQueues.SubmissionProcessingQueue, cancellationToken:default);

        return processingJob;
    }
}