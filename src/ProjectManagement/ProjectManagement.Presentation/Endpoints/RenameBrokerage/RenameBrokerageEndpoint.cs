using Mediator;
using Microsoft.AspNetCore.Mvc;
using Yosef.ProjectManagement.Application.Brokerage.RenameBrokerage;
using GetBrokerageEndpointHandler = ProjectManagement.Presentation.Endpoints.GetBrokerage.GetBrokerage;

namespace ProjectManagement.Presentation.Endpoints.RenameBrokerage;

public static class RenameBrokerageEndpoint
{
    public static void MapRenameBrokerage(this RouteGroupBuilder brokerages)
    {
        brokerages.MapPatch("/{id:guid}/name", async ([FromRoute] Guid id, [FromBody] RenameBrokerageRequest req, ILogger<GetBrokerageEndpointHandler> logger, IMediator mediator) =>
            {
                using var activity = Program.GreeterActivitySource.StartActivity("RenameBrokerage");
                logger.LogInformation("Renaming brokerage {Id}", id);

                req.BrokerageId = id;
                activity?.SetTag("brokerage.id", id);

                return await mediator.Send(req);
            })
            .WithName("RenameBrokerage")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
