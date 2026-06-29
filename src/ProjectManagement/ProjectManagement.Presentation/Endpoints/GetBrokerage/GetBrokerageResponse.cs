namespace ProjectManagement.Presentation.Endpoints.GetBrokerage;

public record GetBrokerageResponse(
    Guid Identifier,
    string BrokerageName,
    string CNPJ,
    string Email,
    string? Phone
);
