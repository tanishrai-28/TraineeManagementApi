using System.ComponentModel.DataAnnotations;

namespace TraineeManagementApi.DTO.SubmissionDTO;

public enum SubmissionStatus
{
    Submitted,
    Resubmitted
}

public class SubmissionResponse{
    public long Id {get; set;}
    public long TaskAssignmentId {get; set;}
    public string SubmissionUrl {get; set;} = string.Empty;
    public string Notes {get; set;} = string.Empty;
    public DateTime SubmittedDate {get; set;}

    [EnumDataType(typeof(SubmissionStatus), ErrorMessage = "Invalid status")]
    public string? Status {get; set;}
}