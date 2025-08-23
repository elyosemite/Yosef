namespace Yosef.ProjectManagement.Domain.Events;

public class OrganizationArchivedEvent : DomainEventBase
{
    public OrganizationArchivedEvent(
        Guid OrganizationId,
        DateTime ArchivedAt)
    {
    }
}
