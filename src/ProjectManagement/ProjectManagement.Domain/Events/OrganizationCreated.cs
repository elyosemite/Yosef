namespace Yosef.ProjectManagement.Domain.Events;

public record OrganizationCreated(
    Guid OrganizationId,
    string Name,
    string Description,
    DateTime CreatedAt);