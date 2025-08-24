using EventProcessor.Settings.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace EventProcessor.RabbitMQ;

public class RabbitMqPublisher : IRabbitMqPublisher, IAsyncDisposable
{
    private readonly IGlobalSettings _globalSettings;
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly object _channelLock = new();

    public RabbitMqPublisher(IGlobalSettings globalSettings, IConnection connection, IChannel channel)
    {
        _globalSettings = globalSettings;
        _connection = connection;
        _channel = channel;
    }

    public static async Task<RabbitMqPublisher> CreateAsync(IGlobalSettings globalSettings)
    {
        var factory = new ConnectionFactory()
        {
            HostName = globalSettings.RabbitMqSettings.HostName,
            Password = globalSettings.RabbitMqSettings.Password,
            UserName = globalSettings.RabbitMqSettings.Username
        };

        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(
            exchange: globalSettings.RabbitMqSettings.ExchangeName,
            type: ExchangeType.Direct,
            durable: true,
            autoDelete: false
        );

        await channel.QueueDeclareAsync(
            queue: globalSettings.RabbitMqSettings.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false
        );

        await channel.QueueBindAsync(
            queue: globalSettings.RabbitMqSettings.QueueName,
            exchange: globalSettings.RabbitMqSettings.ExchangeName,
            routingKey: globalSettings.RabbitMqSettings.QueueName
        );

        return new RabbitMqPublisher(globalSettings, connection, channel);
    }

    public ValueTask PublishAsync<T>(T message)
    {
        var json = JsonSerializer.Serialize(message);
        return PublishPayloadAsync(json);
    }

    public ValueTask PublishPayloadAsync(string payload)
    {
        var body = Encoding.UTF8.GetBytes(payload);

        lock (_channelLock)
        {
            _channel.BasicPublishAsync(
                exchange: _globalSettings.RabbitMqSettings.ExchangeName,
                routingKey: _globalSettings.RabbitMqSettings.QueueName,
                mandatory: true,
                basicProperties: new BasicProperties { },
                body: body,
                new CancellationToken()
            );
        }
        return ValueTask.CompletedTask;
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel.IsOpen)
        {
            await _channel.CloseAsync();
        }
        _channel.Dispose();

        if (_connection.IsOpen)
        {
            await _connection.CloseAsync();
        }
        _connection.Dispose();
    }
}
