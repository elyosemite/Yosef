namespace ProjectManagement.Presentation.Endpoints.CreateBrokerage;

public record BrokerageRequest(string BrokerageName, int ContributorsCount, string? Secret = null);
