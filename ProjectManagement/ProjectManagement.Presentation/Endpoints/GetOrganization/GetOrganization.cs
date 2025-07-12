using AutoMapper;
using ProjectManagement.Infrastructure;

namespace ProjectManagement.Presentation.Endpoints.GetOrganization;

public class GetOrganization
{
    private readonly IOrganizationRepository _organizationRepository;

    public GetOrganization(IOrganizationRepository organizationRepository)
    {
        _organizationRepository = organizationRepository;
    }

    public async Task<IResult> ActionAsync(GetOrganizationRequest req)
    {
        var organization = await _organizationRepository.GetByIdAsync(req.OrganizationIdentifier);

        if (organization == null)
            return TypedResults.NotFound($"Organization with ID {req.OrganizationIdentifier} not found.");

        var dataModel = new GetOrganizationResponse(
            organization.Name,
            organization.ContributorsCount,
            organization.Secret,
            organization.Identifier);
        
        return TypedResults.Ok(dataModel);
    }
}