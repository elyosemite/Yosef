namespace Yosef.ProjectManagement.Domain.Events;

public class BrokerageNameUpdatedEvent(Guid BrokerageId, string BrokerageName) : DomainEventBase
{
    public Guid BrokerageId { get; } = BrokerageId;
    public string BrokerageName { get; } = BrokerageName;
}
