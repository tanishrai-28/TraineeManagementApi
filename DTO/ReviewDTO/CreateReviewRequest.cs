using System.ComponentModel.DataAnnotations;

namespace TraineeManagementApi.DTO.ReviewDTO;

public enum ReviewStatus
{
    Accepted,
    ChangesRequired,
    Rejected
}

public class CreateReviewRequest
{
    [Required]
    public required long SubmissionId { get; set; }

    [Required]
    public required long MentorId { get; set; }

    public string Feedback { get; set; } = string.Empty;

    public int? Score { get; set; }

    [Required]
    [EnumDataType(typeof(ReviewStatus), ErrorMessage = "Invalid status")]
    public required string ReviewStatus{ get; set; }
}