namespace Yosef.ProjectManagement.Domain.Events;

public abstract record DomainEventBase
{
    public DateTime OccurredOn { get; protected set; } = DateTime.UtcNow;
}