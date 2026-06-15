namespace TraineeManagementApi.DTO.TaskAssignmentDTO;

public enum Status
{
    Assigned,
    InProgress,
    Submitted,
    Reviewed,
    Completed
}

public class TaskAssignmentResponse{
    public long Id {get; set;}
    public long TraineeId {get; set;}
    public long MentorId {get; set;}
    public long LearningTaskId {get; set;}
    public DateTime AssignedDate {get; set;}
    public DateTime DueDate {get; set;}
    public  string? Status {get; set;}
    public string Remarks {get; set;} = "";
}