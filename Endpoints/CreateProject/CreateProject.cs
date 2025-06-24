using DotNetEcosystemStudy.Aggregates;
using DotNetEcosystemStudy.Infrastructure;

namespace DotNetEcosystemStudy.Endpoints.CreateProject;

public class CreateProject
{
    private readonly IOrganizationRepository _organizationRepository;

    public CreateProject(IOrganizationRepository organizationRepository)
    {
        _organizationRepository = organizationRepository;
    }

    public async Task<IResult> ActionAsync(CreateProjectRequest req)
    {
        var organization = await _organizationRepository.GetByIdAsync(req.OrganizationIdentifier);

        if (organization == null)
            return TypedResults.NotFound($"Organization with Id {req.OrganizationIdentifier} not found.");

        var project = Project.ProjectFactory(
            req.ProjectName,
            req.Description,
            req.StarsCount,
            req.ForksCount,
            req.ContributorsCount,
            organization.Identifier);

        organization.AddProject(project);
        await _organizationRepository.UpsertAsync(organization);

        var result = await _organizationRepository.GetByIdAsync(req.OrganizationIdentifier);
        return TypedResults.Ok(result);
    }
}