using Microsoft.EntityFrameworkCore;
using TraineeManagementApi.Context;
using TraineeManagementApi.DTO.SubmissionDTO;
using TraineeManagementApi.Models;
using TraineeManagementApi.Services.Interface;

namespace TraineeManagementApi.Services;

public class SubmissionService : ISubmissionService
{
    private readonly ILogger<SubmissionService> _logger;
    private readonly ApplicationDbContext _context;

    public SubmissionService(ApplicationDbContext context, ILogger<SubmissionService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<SubmissionResponse> CreateAsync(CreateSubmissionRequest request)
    {
        var taskAssignmentExists = await _context.TaskAssignments.AnyAsync(x => x.Id == request.TaskAssignmentId);
        if (!taskAssignmentExists)
        {
            throw new ArgumentException("Task assignment does not exists");
        }

        var existingSubmission = await _context.Submissions.FirstOrDefaultAsync(x => x.TaskAssignmentId == request.TaskAssignmentId);

        if (existingSubmission != null)
        {
            existingSubmission.SubmissionUrl = request.SubmissionUrl;
            existingSubmission.Notes = request.Notes;
            existingSubmission.Status = "Resubmitted";
            existingSubmission.SubmittedDate = DateTime.UtcNow;
            existingSubmission.UpdatedDate = DateTime.UtcNow;

            var assignment = await _context.TaskAssignments.FirstOrDefaultAsync(x => x.Id == request.TaskAssignmentId);
            if (assignment != null)
            {
                assignment.Status = "Submitted";
                assignment.UpdatedDate = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return new SubmissionResponse
            {
                Id = existingSubmission.Id,
                TaskAssignmentId = existingSubmission.TaskAssignmentId,
                SubmissionUrl = existingSubmission.SubmissionUrl,
                Notes = existingSubmission.Notes,
                SubmittedDate = existingSubmission.SubmittedDate,
                Status = existingSubmission.Status
            };
        }

        var submission = new Submission()
        {
            TaskAssignmentId = request.TaskAssignmentId,
            SubmissionUrl = request.SubmissionUrl,
            Notes = request.Notes,
            SubmittedDate = DateTime.UtcNow,
            Status = "Submitted",
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        await _context.Submissions.AddAsync(submission);

        var submitAssignment = await _context.TaskAssignments.FirstOrDefaultAsync(x => x.Id == request.TaskAssignmentId);
        if (submitAssignment != null)
        {
            submitAssignment.Status = "Submitted";
            submitAssignment.UpdatedDate = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation($"Submission created.");

        return new SubmissionResponse
        {
            Id = submission.Id,
            TaskAssignmentId = submission.TaskAssignmentId,
            SubmissionUrl = submission.SubmissionUrl,
            Notes = submission.Notes,
            SubmittedDate = submission.SubmittedDate,
            Status = submission.Status
        };
    }

    public async Task<List<SubmissionResponse>> GetAllAsync()
    {
        var submissions = await _context.Submissions.Select(x => new SubmissionResponse
        {
            Id = x.Id,
            TaskAssignmentId = x.TaskAssignmentId,
            SubmissionUrl = x.SubmissionUrl,
            Notes = x.Notes,
            SubmittedDate = x.SubmittedDate,
            Status = x.Status
        }).ToListAsync();

        return submissions;
    }

    public async Task<SubmissionResponse?> GetByIdAsync(long id)
    {
        var submission = await _context.Submissions.FirstOrDefaultAsync(x => x.Id == id);

        if (submission == null)
        {
            _logger.LogInformation($"Submission with {id} not found");
            return null;
        }

        return new SubmissionResponse
        {
            Id = submission.Id,
            TaskAssignmentId = submission.TaskAssignmentId,
            SubmissionUrl = submission.SubmissionUrl,
            Notes = submission.Notes,
            SubmittedDate = submission.SubmittedDate,
            Status = submission.Status
        };
    }
}