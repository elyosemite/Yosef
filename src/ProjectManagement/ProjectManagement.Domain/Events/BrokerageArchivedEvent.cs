namespace Yosef.ProjectManagement.Domain.Events;

public class BrokerageArchivedEvent : DomainEventBase
{
    private readonly Guid _brokerageId;
    private readonly DateTime _archivedAt;
    private readonly bool _active;

    public BrokerageArchivedEvent(
        Guid brokerageId,
        DateTime archivedAt,
        bool active = false)
    {
        _brokerageId = brokerageId;
        _archivedAt = archivedAt;
        _active = active;
    }

    public Guid BrokerageId => _brokerageId;
    public DateTime ArchivedAt => _archivedAt;
    public bool Active => _active;
}
