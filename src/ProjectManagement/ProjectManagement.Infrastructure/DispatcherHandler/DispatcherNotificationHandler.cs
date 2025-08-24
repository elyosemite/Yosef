
using Mediator;
using Microsoft.Extensions.Logging;
using ProjectManagement.Applciation.Repository;
using Yosef.ProjectManagement.Domain.Events;

namespace ProjectManagement.Infrastructure.DispatcherHandler;

public sealed record DispatcherNotification(IEnumerable<DomainEventBase> DomainEvents) : INotification;

public sealed class DispatcherNotificationHandler : INotificationHandler<DispatcherNotification>
{
    private readonly IOutboxRepository _outboxRepository;
    private readonly ILogger<DispatcherNotificationHandler> _logger;

    public DispatcherNotificationHandler(IOutboxRepository outboxRepository, ILogger<DispatcherNotificationHandler> logger)
    {
        _outboxRepository = outboxRepository ?? throw new ArgumentNullException(nameof(outboxRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async ValueTask Handle(DispatcherNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("DispatcherNotificationHandler.Handle() was called!");

            foreach (var domainEvent in notification.DomainEvents)
            {
                _logger.LogInformation("Inserting Domain Event in Outbox table: {@DomainEvent}", domainEvent);

                await _outboxRepository.AddAsync(domainEvent);
            }
    }
}