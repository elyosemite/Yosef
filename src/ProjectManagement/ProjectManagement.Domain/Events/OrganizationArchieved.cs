namespace Yosef.ProjectManagement.Domain.Events;

public record OrganizationArchived(
    Guid OrganizationId,
    DateTime ArchivedAt);