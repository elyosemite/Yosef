namespace Yosef.ProjectManagement.Domain.Events;

public class OrganizationCreatedEvent : DomainEventBase
{
    public OrganizationCreatedEvent(
        Guid OrganizationId,
        string Name,
        string Description,
        DateTime CreatedAt)
    {
    }
}
