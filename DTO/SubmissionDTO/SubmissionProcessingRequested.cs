namespace TraineeManagementApi.DTO.SubmissionDTO;

public class SubmissionProcessingRequested
{
    public Guid MessageId { get; set; }
    public Guid CorrelationId { get; set; }
    public long SubmissionId { get; set; }
    public Guid FileId { get; set; }
    public DateTime RequestedAt { get; set; }
    public string ContractVersion { get; set; } = "v1";
}