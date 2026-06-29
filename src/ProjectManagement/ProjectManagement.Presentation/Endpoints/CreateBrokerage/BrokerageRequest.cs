namespace ProjectManagement.Presentation.Endpoints.CreateBrokerage;

public record BrokerageRequest(string BrokerageName, string CNPJ, string Email, string? Phone = null);
