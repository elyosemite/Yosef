using Mediator;

namespace EventProcessor.Worker;

public class OrgNameUpdatedEvent : INotification
{
    public Guid OrganizationId { get; }
    public string OrganizationName { get; }

    public OrgNameUpdatedEvent(Guid organizationId, string organizationName)
    {
        OrganizationId = organizationId;
        OrganizationName = organizationName;
    }
}