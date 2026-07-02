using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.Repository;
using ProjectManagement.Infrastructure.Metric;

namespace ProjectManagement.Presentation.Endpoints.CreateBrokerage;

public static class CreateBrokerageEndpoint
{
    public static void MapCreateBrokerage(this RouteGroupBuilder brokerages, WebApplication app)
    {
        brokerages.MapPost("/", async ([FromBody] BrokerageRequest req, IValidator<BrokerageRequest> validator, ILogger<CreateBrokerage> logger) =>
            {
                using var activity = Program.GreeterActivitySource.StartActivity("CreateBrokerage");
                logger.LogInformation("Creating brokerage");

                Metrics.CountGreetings.Add(1);

                var brokerageRepository = app.Services.GetRequiredService<IBrokerageRepository>();
                var mapper = app.Services.GetRequiredService<IMapper>();

                await Program.Delay(logger);

                var result = await new CreateBrokerage(brokerageRepository, mapper, logger, validator).ActionAsync(req);

                Metrics.CountBrokeragesCreated.Add(1);
                activity?.SetTag("brokerage.name", req.BrokerageName);

                return result;
            })
            .WithName("CreateBrokerage")
            .Produces<CreateBrokerageResponse>(StatusCodes.Status201Created)
            .Produces<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
