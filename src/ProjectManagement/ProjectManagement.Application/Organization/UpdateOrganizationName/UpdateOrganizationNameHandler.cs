using FluentValidation;
using Mediator;
using Microsoft.Extensions.Logging;
using ProjectManagement.Application.Repository;

namespace Yosef.ProjectManagement.Application.UpdateOrganizationName.Organization;

public class UpdateOrganizationNameHandler : IRequestHandler<UpdateOrganizationNameRequest, UpdateOrganizationNameResponse>
{
    private readonly ILogger<UpdateOrganizationNameHandler> _logger;
    private readonly IValidator<UpdateOrganizationNameRequest> _validator;
    private readonly IOrganizationRepository _organizationRepository;

    public UpdateOrganizationNameHandler(
        ILogger<UpdateOrganizationNameHandler> logger,
        IValidator<UpdateOrganizationNameRequest> validator,
        IOrganizationRepository organizationRepository)
    {
        _logger = logger;
        _validator = validator;
        _organizationRepository = organizationRepository;
    }

    public async ValueTask<UpdateOrganizationNameResponse> Handle(UpdateOrganizationNameRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling request to update organization name: {OrganizationName} for organization ID: {OrganizationId}", request.OrganizationName, request.OrganizationId);
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                _logger.LogWarning("Validation failed for property {Property}: {ErrorMessage}", error.PropertyName, error.ErrorMessage);
            }

            throw new ValidationException(validationResult.Errors);
        }

        var organization = await _organizationRepository.GetByIdAsync(request.OrganizationId);
        _logger.LogInformation("Retrieved organization: {@Organization}", organization);

        if (organization == null)
        {
            _logger.LogWarning("Organization with ID {OrganizationId} not found", request.OrganizationId);
            throw new KeyNotFoundException($"Organization with ID {request.OrganizationId} not found");
        }

        organization.UpdateName(request.OrganizationName);

        await _organizationRepository.UpsertAsync(organization);
        _logger.LogInformation("Organization name updated to: {OrganizationName}", organization.Name);

        UpdateOrganizationNameResponse org = new()
        {
            OrganizationId = organization.Identifier,
            OrganizationName = organization.Name
        };

        _logger.LogInformation("Update successfully organization name: {Title}", org.OrganizationName);

        return org;
    }
}
