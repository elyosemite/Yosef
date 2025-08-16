
using Insurance.Domain.Outbox;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProjectManagement.Infrastructure.Context;
using ProjectManagement.Infrastructure.Repository;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace ProjectManagement.Worker;

public sealed class OutboxProcessorService : BackgroundService
{
    private readonly IServiceProvider _sp;
    private readonly ILogger<OutboxProcessorService> _logger;
    private readonly IConnection _rabbitConnection;
    private readonly IOutboxRepository _outboxRepository;

    public OutboxProcessorService(IServiceProvider sp, ILogger<OutboxProcessorService> logger, IOutboxRepository outboxRepository)
    {
        _sp = sp;
        _logger = logger;
        _outboxRepository = outboxRepository;

        // Configura RabbitMQ
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };
        _rabbitConnection = factory.CreateConnectionAsync();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Outbox Worker iniciado.");

        // verifique se a connection está ok, se não crie.
        
        if (_rabbitConnection.IsOpen)
        {
            _logger.LogInformation("Conexão com RabbitMQ estabelecida.");
        }
        else
        {
            _rabbitConnection = await factory.CreateConnectionAsync();
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<OrganizationContext>();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                // Busca mensagens não processadas
                _logger.LogInformation("Buscando mensagens não processadas no Outbox...");

                var messages = await _outboxRepository.GetPendingAsync(20, stoppingToken);

                if (!messages.Any()) return;

                foreach (var msg in messages)
                {
                    try
                    {
                        // Desserializa
                        var type = Type.GetType(msg.Type);
                        if (type == null) throw new InvalidOperationException($"Tipo não encontrado: {msg.Type}");

                        var @event = JsonSerializer.Deserialize(msg.Payload, type);
                        if (@event == null) throw new InvalidOperationException("Erro de desserialização de evento vindo do RabbitMQ");

                        // Publica no RabbitMQ
                        await PublishToRabbitAsync(routingKey: msg.Type, jsonPayload: msg.Payload);

                        // (Opcional) Tratar internamente no Worker
                        await mediator.Publish(@event, stoppingToken);

                        msg.ProcessedOn = DateTime.UtcNow;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Erro ao processar mensagem {Id}", msg.Id);
                        msg.Error = ex.ToString();
                    }
                }
                await db.SaveChangesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no loop principal do OutboxWorker");
            }

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }

    private async Task PublishToRabbitAsync(string routingKey, string jsonPayload)
    {
        using var channel = await _rabbitConnection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(exchange: "domain_events", type: ExchangeType.Topic, durable: true);

        var body = Encoding.UTF8.GetBytes(jsonPayload);

        var props = new BasicProperties();

        await channel.BasicPublishAsync(
            exchange: "domain_events",
            routingKey: routingKey,
            mandatory: true,
            basicProperties: props,
            body: body
        );

        _logger.LogInformation("Evento publicado no RabbitMQ: {RoutingKey}", routingKey);
    }

    public override void Dispose()
    {
        _rabbitConnection.Dispose();
        base.Dispose();
    }
}
