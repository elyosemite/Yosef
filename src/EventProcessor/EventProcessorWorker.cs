using EventProcessor.Repository;
using Mediator;
using System.Text.Json;

namespace EventProcessor.Worker;

public sealed class EventProcessorWorker : BackgroundService
{
    private readonly IServiceProvider _sp;
    private readonly ILogger<EventProcessorWorker> _logger;
    private readonly IOutboxRepository _outboxRepository;

    public EventProcessorWorker(IServiceProvider sp, ILogger<EventProcessorWorker> logger, IOutboxRepository outboxRepository)
    {
        _sp = sp;
        _logger = logger;
        _outboxRepository = outboxRepository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Outbox Worker iniciado.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _sp.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                _logger.LogInformation("Fetching pending messages from Outbox...");

                var messages = await _outboxRepository.GetPendingAsync(20, stoppingToken);

                _logger.LogInformation("Found {@messages} pending messages in Outbox.", messages);

                if (!messages.Any()) return;

                foreach (var msg in messages)
                {
                    try
                    {
                        var type = Type.GetType(msg.Type);
                        if (type == null) throw new InvalidOperationException($"Type does not set up: {msg.Type}");

                        var @event = JsonSerializer.Deserialize(msg.Payload, type);
                        if (@event == null) throw new InvalidOperationException("Error to deserialize event from Outbox to RabbitMQ");

                        await mediator.Publish(@event, stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Erro ao processar mensagem {Id}", msg.Id);
                        msg.Error = ex.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no loop principal do OutboxWorker");
            }

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
}
