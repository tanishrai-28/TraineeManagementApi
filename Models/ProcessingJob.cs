namespace TraineeManagementApi.Models;

public enum ProcessingJobEnumValues {Queued, Processing, Completed, Failed}
public class ProcessingJob
{
    public Guid Id {get; set;}
    public int Attempts{get; set;} = 0;
    public Guid CorelationId{get; set;}
    public long SubmissionId{get; set;}
    public Guid SubmissionFileId {get; set;}
    public Guid MessageId {get; set;}
    public string? ErrorSummary {get; set;}
    public required string Status {get; set;}
    public DateTime StartedTime {get; set;}
    public DateTime CompletedTime {get; set;}
    public DateTime CreatedAt {get; set;}
    public DateTime UpdatedAt {get; set;}
}