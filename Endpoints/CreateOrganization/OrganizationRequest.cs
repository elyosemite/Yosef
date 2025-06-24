namespace DotNetEcosystemStudy.Endpoints.CreateOrganization ;

public record OrganizationRequest(string OrganizationName, int ContributorsCount, string? Secret = null);
