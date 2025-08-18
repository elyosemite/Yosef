namespace EventProcessor.Repository;

public interface IOutboxRepository
{
    Task AddAsync(DomainEventBase domainEvent, CancellationToken ct = default);
    Task<IEnumerable<OutboxMessage>> GetPendingAsync(int limit, CancellationToken ct = default);
    Task MarkProcessedAsync(Guid id, CancellationToken ct = default);
    Task MarkOccurredAsync(Guid id, CancellationToken ct = default);
    Task MarkErrorAsync(Guid id, string error, CancellationToken ct = default);
}
