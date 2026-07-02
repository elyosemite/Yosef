using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.Repository;

namespace ProjectManagement.Presentation.Endpoints.GetBrokerage;

public static class GetBrokerageEndpoint
{
    public static void MapGetBrokerage(this RouteGroupBuilder brokerages, WebApplication app)
    {
        brokerages.MapGet("/{id:guid}", async ([FromRoute] Guid id, ILogger<GetBrokerage> logger) =>
            {
                using var activity = Program.GreeterActivitySource.StartActivity("GetBrokerage");
                logger.LogInformation("Get brokerage by id = {Id}", id);

                var brokerageRepository = app.Services.GetRequiredService<IBrokerageRepository>();

                await Program.Delay(logger);

                activity?.SetTag("brokerage.id", id);

                return await new GetBrokerage(brokerageRepository)
                    .ActionAsync(new GetBrokerageRequest(id));
            })
            .WithName("GetBrokerage")
            .Produces<GetBrokerageResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
