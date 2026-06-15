namespace TraineeManagementApi.Models;

public class Review
{
    public long Id { get; set; }
    public required long SubmissionId { get; set; }
    public Submission? Submission { get; set; }
    public required long MentorId { get; set; }
    public Mentor? Mentor { get; set; }
    public required string Feedback { get; set; } = "";
    public int? Score { get; set; }
    public required string ReviewStatus { get; set; }
    public required DateTime ReviewedDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}