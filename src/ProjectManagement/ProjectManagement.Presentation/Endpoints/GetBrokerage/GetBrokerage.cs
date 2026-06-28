using ProjectManagement.Application.Repository;

namespace ProjectManagement.Presentation.Endpoints.GetBrokerage;

public class GetBrokerage
{
    private readonly IBrokerageRepository _brokerageRepository;

    public GetBrokerage(IBrokerageRepository brokerageRepository)
    {
        _brokerageRepository = brokerageRepository;
    }

    public async Task<IResult> ActionAsync(GetBrokerageRequest req)
    {
        var brokerage = await _brokerageRepository.GetByIdAsync(req.BrokerageIdentifier);

        if (brokerage is null)
            return TypedResults.NotFound($"Brokerage with ID {req.BrokerageIdentifier} not found.");

        var response = new GetBrokerageResponse(
            brokerage.Name,
            brokerage.ContributorsCount,
            brokerage.Secret,
            brokerage.Identifier);

        return TypedResults.Ok(response);
    }
}
