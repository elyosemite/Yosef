using DotNetEcosystemStudy.src.Domain.Aggregates;
using DotNetEcosystemStudy.src.Infrastructure;
using AutoMapper;

namespace DotNetEcosystemStudy.src.Presentation.Endpoints.CreateOrganization;

public class CreateOrganization
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IMapper _mapper;

    public CreateOrganization(IOrganizationRepository organizationRepository, IMapper mapper)
    {
        _organizationRepository = organizationRepository;
        _mapper = mapper;
    }

    public async Task<IResult> ActionAsync(OrganizationRequest req)
    {
        var organization = Organization.OrganizationFactory(req.OrganizationName, req.ContributorsCount);

        if (string.IsNullOrEmpty(req.Secret))
            return TypedResults.BadRequest("Secret cannot be null or empty.");

        organization.UpdateSecret(req.Secret);

        var result = await _organizationRepository.CreateAsync(organization);

        var dataModel = new CreateOrganizationResponse(result.Identifier, result.Name);

        return TypedResults.Ok(dataModel);
    }
}
