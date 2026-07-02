using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.Repository;

namespace ProjectManagement.Presentation.Endpoints.CreateProject;

public static class CreateProjectEndpoint
{
    public static void MapCreateProject(this RouteGroupBuilder brokerages, WebApplication app)
    {
        brokerages.MapPost("/{id:guid}/projects", async ([FromRoute] Guid id, [FromBody] CreateProjectRequest req, ILogger<CreateProject> logger) =>
            {
                using var activity = Program.GreeterActivitySource.StartActivity("CreateProject");
                logger.LogInformation("Creating project for brokerage {Id}", id);

                var brokerageRepository = app.Services.GetRequiredService<IBrokerageRepository>();

                req = req with { OrganizationIdentifier = id };
                activity?.SetTag("brokerage.id", id);

                return await new CreateProject(brokerageRepository).ActionAsync(req);
            })
            .WithName("CreateProject")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
