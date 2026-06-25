using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using TraineeManagementApi.Configurations;
using TraineeManagementApi.Context;
using TraineeManagementApi.DTO.SubmissionDTO;
using TraineeManagementApi.DTO.SubmissionFileDTO;
using TraineeManagementApi.Models;
using TraineeManagementApi.Services.RabbitMq;

namespace TraineeManagementApi.Services.FileStorage;

public class SubmissionFileService : ISubmissionFileService
{
    private readonly ILogger<SubmissionFileService> _logger;
    private readonly ApplicationDbContext _context;
    private readonly IFileStorageService _storage;
    private readonly IRabbitMqPublisher _rabbitMqPublisher;

    public SubmissionFileService(ApplicationDbContext context, IFileStorageService storage, IRabbitMqPublisher rabbitMqPublisher, ILogger<SubmissionFileService> logger)
    {
        _context = context;
        _storage = storage;
        _rabbitMqPublisher = rabbitMqPublisher;
        _logger = logger;
    }

    public async Task<SubmissionFileResponse> UploadAsync(long submissionId, IFormFile file, string uploadedBy, CancellationToken cancellationToken = default)
    {
        if(file == null)
        {
            throw new Exception("File not attached");
        }

        if(file.Length == 0)
        {
            throw new Exception("Empty file entered");
        }

        if(file.Length > 10 * 1024 * 1024)
        {
            throw new Exception("File size cannot get greater than 10MB");
        }

        string[] allowedExtensions = {".pdf", ".jpg", ".docx"};
        var extension = Path.GetExtension(file.FileName).ToLower();
        if(!allowedExtensions.Contains(extension))
        {
            throw new Exception("Extension not allowed");
        }


        var storageName = await _storage.SaveAsync(file.OpenReadStream(), extension, cancellationToken);

        var entity = new SubmissionFile
        {
            SubmisionId = submissionId,
            OriginalFileName = file.FileName,
            GeneratedFileName = storageName,
            ContentType = file.ContentType,
            Size = file.Length,
            Checksum = "",
            UploadedByUser = uploadedBy,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        _context.SubmissionFiles.Add(entity);
        await _context.SaveChangesAsync();

        var messageId = Guid.NewGuid();
        var correlationId = Guid.NewGuid();

        var job = new ProcessingJob
        {
            MessageId = messageId,
            CorelationId = correlationId,
            SubmissionId = submissionId,
            SubmissionFileId = entity.Id,
            Status = "Queued",
            Attempts = 0,
            CreatedAt = DateTime.UtcNow,
        };

        _context.ProcessingJobs.Add(job);
        await _context.SaveChangesAsync();

        var message = new SubmissionProcessingRequested
        {
            MessageId = messageId,
            CorrelationId = correlationId,
            SubmissionId = submissionId,
            FileId = entity.Id,
            RequestedAt = DateTime.UtcNow
        };

        await _rabbitMqPublisher.PublishAsync(message, RabbitMQQueues.SubmissionProcessingQueue, cancellationToken);

        return new SubmissionFileResponse
        {
            Id = entity.Id,
            OriginalFileName = entity.OriginalFileName,
            SubmissionId = entity.SubmisionId,
            ContentType = entity.ContentType,
            Size = entity.Size,
            UploadedAt = entity.CreatedDate,
            UploadedByUser = entity.UploadedByUser
        };
    }

    public async Task<SubmissionFileDownloadDto> DownloadAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var file = await _context.SubmissionFiles.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if(file == null)
        {
            throw new KeyNotFoundException("File metadata not found");
        }

        var exists = await _storage.ExistsAsync(file.GeneratedFileName);

        if(!exists)
        {
            throw new FileNotFoundException("File not found");
        }

        var stream = await _storage.OpenReadAsync(file.GeneratedFileName, cancellationToken);

        return new SubmissionFileDownloadDto
        {
            Stream = stream,
            ContentType = file.ContentType,
            FileName = file.OriginalFileName
        };
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var file = await _context.SubmissionFiles.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if(file == null)
        {
            throw new KeyNotFoundException("File metadata not found");
        }

        var exists = await _storage.ExistsAsync(file.GeneratedFileName);
        if(exists)
        {
            await _storage.DeleteAsync(file.GeneratedFileName);
        }

        _context.SubmissionFiles.Remove(file);
        await _context.SaveChangesAsync();
    }

}