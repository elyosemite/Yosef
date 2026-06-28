namespace ProjectManagement.Presentation.Endpoints.GetBrokerage;

public record GetBrokerageResponse(
    string BrokerageName,
    int ContributorsCount,
    string? Secret,
    Guid Identifier
);
