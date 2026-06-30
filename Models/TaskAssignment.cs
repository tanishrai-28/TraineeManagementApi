namespace TraineeManagementApi.Models;

public class TaskAssignment
{
    public long Id { get; set; }
    public required long TraineeId { get; set; }
    public required long MentorId { get; set; }
    public required long LearningTaskId { get; set; }
    public required DateTime AssignedDate { get; set; }
    public required DateTime DueDate { get; set; }
    public required string Status { get; set; }
    public string Remarks { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}