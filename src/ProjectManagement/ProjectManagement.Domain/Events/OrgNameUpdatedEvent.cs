namespace Yosef.ProjectManagement.Domain.Events;

public class OrgNameUpdatedEvent(Guid OrganizationId, string OrganizationName) : DomainEventBase
{
    public Guid OrganizationId { get; } = OrganizationId;
    public string OrganizationName { get; } = OrganizationName;
}
