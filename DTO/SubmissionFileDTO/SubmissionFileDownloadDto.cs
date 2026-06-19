namespace TraineeManagementApi.DTO.SubmissionFileDTO;

public class SubmissionFileDownloadDto
{
    public Stream Stream {get; set;} = null!;
    public string FileName {get; set;} = "";
    public string ContentType{get; set;} = "application/octet-stream";
}