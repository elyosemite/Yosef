using ProjectManagement.Domain.Aggregates;
using AutoMapper;
using FluentValidation;
using Yosef.ProjectManagement.Domain.Outbox;
using System.Text.Json;
using ProjectManagement.Application.Repository;

namespace ProjectManagement.Presentation.Endpoints.CreateOrganization;

public class CreateOrganization
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateOrganization> _logger;
    private readonly IValidator<OrganizationRequest> _validator;

    public CreateOrganization(IOrganizationRepository organizationRepository, IMapper mapper, ILogger<CreateOrganization> logger, IValidator<OrganizationRequest> validator)
    {
        _organizationRepository = organizationRepository;
        _mapper = mapper;
        _logger = logger;
        _validator = validator;
    }

    public async Task<IResult> ActionAsync(OrganizationRequest req)
    {
        _logger.LogInformation("Creating organization with name: {OrganizationName} and contributors count: {ContributorsCount}", req.OrganizationName, req.ContributorsCount);
        var validationResult = await _validator.ValidateAsync(req);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for organization creation: {Errors}", validationResult.Errors);

            var detailedProblem = new HttpValidationProblemDetails(validationResult.ToDictionary())
            {
                Title = "Validation Error",
                Status = StatusCodes.Status400BadRequest,
                Detail = "One or more validation errors occurred.",
                Instance = req.OrganizationName
            };

            return TypedResults.BadRequest(detailedProblem);
        }

        var organization = Organization.OrganizationFactory(req.OrganizationName, req.ContributorsCount);

        organization.UpdateSecret(req.Secret!);

        // Capture the Domain Events
        var domainEvents = organization.DomainEvents.AsEnumerable();

        foreach (var domainEvent in domainEvents)
        {
            // Create the Outbox Message
            _logger.LogInformation("Domain Event: {@DomainEvent}", domainEvent);
            var outboxMessage = new OutboxMessage
            {
                Type = domainEvent.GetType().FullName!,
                Payload = JsonSerializer.Serialize(domainEvent),
                OccurredOn = DateTime.UtcNow
            };
        }

        var result = await _organizationRepository.CreateAsync(organization);

        var dataModel = new CreateOrganizationResponse(result.Identifier, result.Name);
        _logger.LogInformation("Organization successfully created {@organization}", dataModel);

        return TypedResults.Ok(dataModel);
    }
}
