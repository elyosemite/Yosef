namespace Yosef.ProjectManagement.Domain.Events;

public abstract class DomainEventBase
{
    public DateTime OccurredOn { get; protected set; } = DateTime.UtcNow;
}