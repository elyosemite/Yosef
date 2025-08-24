namespace EventProcessor.RabbitMQ;

public interface IRabbitMqPublisher
{
    ValueTask PublishAsync<T>(T message);
    ValueTask PublishPayloadAsync(string payload);
}