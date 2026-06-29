using ProjectManagement.Domain.Aggregates;
using AutoMapper;
using FluentValidation;
using ProjectManagement.Application.Repository;

namespace ProjectManagement.Presentation.Endpoints.CreateBrokerage;

public class CreateBrokerage
{
    private readonly IBrokerageRepository _brokerageRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateBrokerage> _logger;
    private readonly IValidator<BrokerageRequest> _validator;

    public CreateBrokerage(IBrokerageRepository brokerageRepository, IMapper mapper, ILogger<CreateBrokerage> logger, IValidator<BrokerageRequest> validator)
    {
        _brokerageRepository = brokerageRepository;
        _mapper = mapper;
        _logger = logger;
        _validator = validator;
    }

    public async Task<IResult> ActionAsync(BrokerageRequest req)
    {
        _logger.LogInformation("Creating brokerage with name: {BrokerageName}", req.BrokerageName);

        var validationResult = await _validator.ValidateAsync(req);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for brokerage creation: {Errors}", validationResult.Errors);

            var problem = new HttpValidationProblemDetails(validationResult.ToDictionary())
            {
                Title = "Validation Error",
                Status = StatusCodes.Status400BadRequest,
                Detail = "One or more validation errors occurred.",
            };

            return TypedResults.BadRequest(problem);
        }

        var brokerage = Brokerage.BrokerageFactory(req.BrokerageName, req.CNPJ, req.Email, req.Phone);

        var result = await _brokerageRepository.CreateAsync(brokerage);

        var response = new CreateBrokerageResponse(result.Identifier, result.Name);
        _logger.LogInformation("Brokerage successfully created: {@Brokerage}", response);

        return TypedResults.Created($"/api/v1/brokerages/{result.Identifier}", response);
    }
}
