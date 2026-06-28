namespace Yosef.ProjectManagement.Domain.Events;

public class BrokerageCreatedEvent : DomainEventBase
{
    private readonly Guid _brokerageId;
    private readonly string _name;
    private readonly string _description;
    private readonly DateTime _createdAt;

    public Guid BrokerageId => _brokerageId;
    public string Name => _name;
    public string Description => _description;
    public DateTime CreatedAt => _createdAt;

    public BrokerageCreatedEvent(
        Guid brokerageId,
        string name,
        string description,
        DateTime createdAt)
    {
        _brokerageId = brokerageId;
        _name = name;
        _description = description;
        _createdAt = createdAt;
    }
}
