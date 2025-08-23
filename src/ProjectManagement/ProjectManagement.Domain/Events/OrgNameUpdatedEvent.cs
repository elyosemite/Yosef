namespace Yosef.ProjectManagement.Domain.Events;

public class OrgNameUpdatedEvent : DomainEventBase
{
    public OrgNameUpdatedEvent(Guid OrganizationId, string OrganizationName)
    {
    }
}
