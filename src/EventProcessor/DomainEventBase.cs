namespace EventProcessor;

public abstract class DomainEventBase
{
    public DateTime OccurredOn { get; protected set; } = DateTime.UtcNow;
}