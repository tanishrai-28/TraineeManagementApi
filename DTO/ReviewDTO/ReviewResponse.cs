using System.ComponentModel.DataAnnotations;

namespace TraineeManagementApi.DTO.ReviewDTO;


public class ReviewResponse{
    public long Id {get; set;}
    public long SubmissionId {get; set;}
    public long MentorId {get; set;}
    public string Feedback {get; set;} = string.Empty;
    public int? Score {get; set;}

    [EnumDataType(typeof(ReviewStatus), ErrorMessage = "Invalid status")]
    public string? ReviewStatus {get; set;}
    public DateTime ReviewedDate {get; set;}
}