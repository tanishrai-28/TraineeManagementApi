namespace TraineeManagementApi.Configurations;

public class RabbitMQSettings
{
    public string HostName { get; set; } = string.Empty;
    public string VirtualHost { get; set; } = string.Empty;
    public int Port { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public static class RabbitMQQueues
{
    public const string SubmissionProcessingQueue = "submission-processing";
}