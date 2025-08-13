namespace Yosef.ProjectManagement.Domain.Events;

public record OrganizationArchivedEvent(
    Guid OrganizationId,
    DateTime ArchivedAt);