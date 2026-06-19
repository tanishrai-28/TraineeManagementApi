namespace TraineeManagementApi.Models;

public class SubmissionFile
{
    public Guid Id { get; set; }
    public required long SubmisionId {get; set;}
    public required string OriginalFileName { get; set; }
    public required string GeneratedFileName { get; set; }
    public required string ContentType { get; set; }
    public required long Size { get; set; }
    public required string Checksum { get; set; }
    public required string UploadedByUser { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}