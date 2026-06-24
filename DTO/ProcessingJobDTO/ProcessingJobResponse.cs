namespace TraineeManagementApi.DTO.ProcessingJobDTO;

public class ProcessingJobResponse
{
    public Guid Id {get; set;}
    public Guid MessageId{get; set;}
    public Guid CorrelationId{get; set;}
    public string Status{get; set;} = "";
    public int Attempts{get; set;}
    public string? ErrorSummary {get; set;}
    public DateTime? StartedTime {get; set;}
    public DateTime? CompletedTime{get; set;}
}