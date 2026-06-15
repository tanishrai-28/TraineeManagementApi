namespace TraineeManagementApi.Models;

public class Submission
{
    public long Id { get; set; }
    public required long TaskAssignmentId { get; set; }
    public required string SubmissionUrl { get; set; } = string.Empty;
    public required string Notes { get; set; } = string.Empty;
    public required DateTime SubmittedDate { get; set; }
    public required string Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}