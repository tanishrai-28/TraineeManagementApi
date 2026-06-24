using Microsoft.EntityFrameworkCore;
using TraineeManagementApi.Context;
using TraineeManagementApi.DTO.ReviewDTO;
using TraineeManagementApi.Models;
using TraineeManagementApi.Services.Interface;

namespace TraineeManagementApi.Services;

public class ReviewService : IReviewService
{
    private readonly ILogger<ReviewService> _logger;
    private readonly ApplicationDbContext _context;

    public ReviewService(ApplicationDbContext context, ILogger<ReviewService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ReviewResponse> CreateAsync(CreateReviewRequest request)
    {
        var submission = await _context.Submissions.FirstOrDefaultAsync(x => x.Id == request.SubmissionId);
        if (submission == null)
        {
            _logger.LogWarning($"Submission for required review does not exist");
            throw new ArgumentException("Submission does not exists");
        }

        var mentorExists = await _context.Mentors.AnyAsync(x => x.Id == request.MentorId);
        if (!mentorExists)
        {
            _logger.LogWarning($"Mentor for submission does not exist");
            throw new ArgumentException("Mentor does not exists");
        }

        var existingReview = await _context.Reviews.FirstOrDefaultAsync(x => x.SubmissionId == request.SubmissionId);

        if (existingReview != null)
        {
            existingReview.Feedback = request.Feedback;
            existingReview.Score = request.Score;
            existingReview.ReviewStatus = request.ReviewStatus;
            existingReview.ReviewedDate = DateTime.UtcNow;
            existingReview.UpdatedDate = DateTime.UtcNow;

            var updateAssignment = await _context.TaskAssignments.FirstOrDefaultAsync(x => x.Id == submission.TaskAssignmentId);
            if (updateAssignment != null)
            {
                updateAssignment.Status = request.ReviewStatus == "Accepted" ? "Completed" : "Reviewed";
                updateAssignment.UpdatedDate = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();


            return new ReviewResponse
            {
                Id = existingReview.Id,
                SubmissionId = existingReview.SubmissionId,
                MentorId = existingReview.MentorId,
                Feedback = existingReview.Feedback,
                Score = existingReview.Score,
                ReviewStatus = existingReview.ReviewStatus,
                ReviewedDate = existingReview.ReviewedDate
            };
        }

        var review = new Review()
        {
            SubmissionId = request.SubmissionId,
            MentorId = request.MentorId,
            Feedback = request.Feedback,
            Score = request.Score,
            ReviewedDate = DateTime.UtcNow,
            ReviewStatus = request.ReviewStatus,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        await _context.Reviews.AddAsync(review);

        var assignment = await _context.TaskAssignments.FirstOrDefaultAsync(x => x.Id == submission.TaskAssignmentId);
        if (assignment != null)
        {
            assignment.Status = request.ReviewStatus == "Accepted" ? "Completed" : "Reviewed";
            assignment.UpdatedDate = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation($"Review created.");

        return new ReviewResponse
        {
            Id = review.Id,
            SubmissionId = review.SubmissionId,
            MentorId = review.MentorId,
            Feedback = review.Feedback,
            Score = review.Score,
            ReviewStatus = review.ReviewStatus,
            ReviewedDate = review.ReviewedDate
        };
    }

    public async Task<List<ReviewResponse>> GetAllAsync()
    {
        var reviews = await _context.Reviews.Select(x => new ReviewResponse
        {
            Id = x.Id,
            SubmissionId = x.SubmissionId,
            MentorId = x.MentorId,
            Feedback = x.Feedback,
            Score = x.Score,
            ReviewStatus = x.ReviewStatus,
            ReviewedDate = x.ReviewedDate            
        }).ToListAsync();

        _logger.LogInformation("Fetched all reviews");

        return reviews;
    }

    public async Task<ReviewResponse?> GetByIdAsync(long id)
    {
        var review = await _context.Reviews.FirstOrDefaultAsync(x => x.Id == id);

        if (review == null)
        {
            _logger.LogWarning($"Review with {id} not found");
            return null;
        }

        return new ReviewResponse
        {
            Id = review.Id,
            SubmissionId = review.SubmissionId,
            MentorId = review.MentorId,
            Feedback = review.Feedback,
            Score = review.Score,
            ReviewStatus = review.ReviewStatus,
            ReviewedDate = review.ReviewedDate
        };
    }
}