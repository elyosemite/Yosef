using Mediator;
using Microsoft.Extensions.Logging;
using ProjectManagement.Applciation.Repository;
using Yosef.ProjectManagement.Domain.Events;

namespace ProjectManagement.Infrastructure.DispatcherHandler;

public class DispatcherNotification : INotification
{
    public IEnumerable<DomainEventBase> DomainEvents { get; }

    public DispatcherNotification(IEnumerable<DomainEventBase> domainEvents)
    {
        DomainEvents = domainEvents ?? throw new ArgumentNullException(nameof(domainEvents));
    }
}

public class DispatcherHandler : INotificationHandler<DispatcherNotification>
{
    private readonly IOutboxRepository _outboxRepository;
    private readonly ILogger<DispatcherHandler> _logger;

    public DispatcherHandler(IOutboxRepository outboxRepository, ILogger<DispatcherHandler> logger)
    {
        _outboxRepository = outboxRepository ?? throw new ArgumentNullException(nameof(outboxRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async ValueTask Handle(DispatcherNotification notification, CancellationToken cancellationToken)
    {
        foreach (var domainEvent in notification.DomainEvents)
        {
            _logger.LogInformation("Inserting Domain Event in Outbox table: {@DomainEvent}", domainEvent);

            await _outboxRepository.AddAsync(domainEvent);
        }
    }
}