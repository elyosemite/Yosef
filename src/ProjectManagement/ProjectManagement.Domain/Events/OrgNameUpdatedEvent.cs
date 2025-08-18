namespace Yosef.ProjectManagement.Domain.Events;

public record OrgNameUpdatedEvent(Guid OrganizationId, string OrganizationName) : DomainEventBase;
