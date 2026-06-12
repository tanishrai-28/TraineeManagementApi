namespace TraineeManagementApi.DTO.MentorDTO;

public class MentorResponse{
    public long Id {get; set;}
    public required string FirstName {get; set;}
    public required string LastName {get; set;}
    public required string Email {get; set;}
    public required string Expertise {get; set;}
    public required string Status {get; set;}
    public DateTime CreatedDate {get; set;}
    public DateTime UpdatedDate {get; set;}
}