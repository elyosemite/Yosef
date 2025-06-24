namespace DotNetEcosystemStudy.Endpoints.GetOrganization;

public record GetOrganizationResponse
(
    string OrganizationName,
    int ContributorsCount,
    string? Secret,
    Guid Identifier
);
