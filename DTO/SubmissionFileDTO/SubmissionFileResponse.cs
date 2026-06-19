namespace TraineeManagementApi.DTO.SubmissionFileDTO;

public class SubmissionFileResponse
{
    public Guid Id {get; set;}
    public long SubmissionId {get; set;}
    public string OriginalFileName {get; set;} = "";
    public string ContentType {get; set;} = "";
    public long Size {get; set;}
    public string UploadedByUser {get; set;} = "";
    public DateTime UploadedAt {get; set;}
}