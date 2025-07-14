namespace ProjectManagement.Presentation.Endpoints.CreateProject;

public record CreateProjectRequest(
    Guid OrganizationIdentifier,
    string ProjectName,
    int ContributorsCount,
    string Description,
    int StarsCount,
    int ForksCount
);