namespace DotNetEcosystemStudy.Endpoints;

public record OrganizationRequest(string OrganizationName, int ContributorsCount, string? Secret = null);
