using Mediator;

namespace EventProcessor.Events;

public class OrgNameUpdatedEvent : DomainEventBase, INotification
{
    public Guid OrganizationId { get; }
    public string OrganizationName { get; }

    public OrgNameUpdatedEvent(Guid organizationId, string organizationName)
    {
        OrganizationId = organizationId;
        OrganizationName = organizationName;
    }
}