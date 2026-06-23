using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace TraineeManagementApi.Services.RabbitMq;

public class RabbitMqPublisher : IRabbitMqPublisher
{
    private readonly ConnectionFactory _connection;
    private readonly ILogger<RabbitMqPublisher> _logger;

    public RabbitMqPublisher(ConnectionFactory connection, ILogger<RabbitMqPublisher> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    public async Task PublishAsync<T>(T message, string queueName)
    {
        try
        {
            using var connection = await _connection.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false
            );

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            var properties = new BasicProperties
            {
                Persistent = true
            };

            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: queueName,
                mandatory: false,
                basicProperties: properties,
                body: body
            );
        }
        catch (Exception)
        {
            _logger.LogInformation("RabbitMQ offline");
        }

    }
}