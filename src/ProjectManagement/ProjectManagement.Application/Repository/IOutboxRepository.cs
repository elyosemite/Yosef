using Yosef.ProjectManagement.Domain.Events;
using Yosef.ProjectManagement.Domain.Outbox;

namespace ProjectManagement.Applciation.Repository;

public interface IOutboxRepository
{
    Task AddAsync(DomainEventBase domainEvent, CancellationToken ct = default);
    Task<IEnumerable<OutboxMessage>> GetPendingAsync(int limit, CancellationToken ct = default);
    Task MarkProcessedAsync(Guid id, CancellationToken ct = default);
    Task MarkErrorAsync(Guid id, string error, CancellationToken ct = default);
}
