namespace Yosef.ProjectManagement.Domain.Events;

public class OrganizationArchivedEvent : DomainEventBase
{
    private readonly Guid _organizationId;
    private readonly DateTime _archivedAt;
    private readonly bool _active;

    public OrganizationArchivedEvent(
        Guid OrganizationId,
        DateTime ArchivedAt,
        bool active = false)
    {
        _organizationId = OrganizationId;
        _archivedAt = ArchivedAt;
        _active = active;
    }

    public Guid OrganizationId => _organizationId;
    public DateTime ArchivedAt => _archivedAt;
    public bool Active => _active;
}
