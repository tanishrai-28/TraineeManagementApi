namespace TraineeManagementApi.Services.RabbitMq;

public interface IRabbitMqPublisher
{
    Task PublishAsync<T> (T message, string queueName);
}