using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace EventProcessor.RabbitMQ;

public class RabbitMqPublisher : IRabbitMqPublisher, IAsyncDisposable
{
    private IConnection _connection;
    private IChannel _channel;
    private readonly string _hostname;
    private readonly string _queueName;

    public IConnection Connection { get => _connection; set => _connection = value; }
    public IChannel Channel { get => _channel; set => _channel = value; }

    public RabbitMqPublisher(string hostname = "rabbitmq", string queueName = "domain_events")
    {
        _hostname = hostname;
        _queueName = queueName;
    }

    public async Task InitializeAsync()
    {
        var factory = new ConnectionFactory()
        {
            HostName = _hostname,
            Password = "guest",
            UserName = "guest"
        };
        
        Connection = await factory.CreateConnectionAsync();
        Channel = await Connection.CreateChannelAsync();

        await Channel.QueueDeclareAsync(
            queue: _queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );
    }

    public async ValueTask PublishAsync<T>(T message)
    {
        if (Channel == null)
            throw new InvalidOperationException("RabbitMQ channel is not initialized. Call InitializeAsync first.");

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        await Channel.BasicPublishAsync(
            exchange: "domain_events",
            routingKey: _queueName,
            mandatory: true,
            basicProperties: new BasicProperties {},
            body: body,
            new CancellationToken()
        );
    }

    public async ValueTask DisposeAsync()
    {
        if (Channel != null)
        {
            await Task.Run(async () => await Channel.CloseAsync());
            Channel.Dispose();
        }

        if (Connection != null)
        {
            await Task.Run(async () => await Connection.CloseAsync());
            Connection.Dispose();
        }
    }
}
