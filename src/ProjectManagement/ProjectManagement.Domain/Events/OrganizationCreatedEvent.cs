namespace Yosef.ProjectManagement.Domain.Events;

public record OrganizationCreatedEvent(
    Guid OrganizationId,
    string Name,
    string Description,
    DateTime CreatedAt) : DomainEventBase;