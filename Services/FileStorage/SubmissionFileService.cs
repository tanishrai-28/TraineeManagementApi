using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using TraineeManagementApi.Context;
using TraineeManagementApi.DTO.SubmissionFileDTO;
using TraineeManagementApi.Models;

namespace TraineeManagementApi.Services.FileStorage;

public class SubmissionFileService : ISubmissionFileService
{
    private readonly ILogger<SubmissionFileService> _logger;
    private readonly ApplicationDbContext _context;
    private readonly IFileStorageService _storage;

    public SubmissionFileService(ApplicationDbContext context, IFileStorageService storage, ILogger<SubmissionFileService> logger)
    {
        _context = context;
        _storage = storage;
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

        using var sha = SHA256.Create();

        var hash = Convert.ToHexString(sha.ComputeHash(file.OpenReadStream()));

        var storageName = await _storage.SaveAsync(file.OpenReadStream(), extension, cancellationToken);

        var entity = new SubmissionFile
        {
            SubmisionId = submissionId,
            OriginalFileName = file.FileName,
            GeneratedFileName = storageName,
            ContentType = file.ContentType,
            Size = file.Length,
            Checksum = hash,
            UploadedByUser = uploadedBy,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        _context.SubmissionFiles.Add(entity);
        await _context.SaveChangesAsync();

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