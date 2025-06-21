using DotNetEcosystemStudy.DataModel;
using DotNetEcosystemStudy.Aggregates;
using DotNetEcosystemStudy.Settings.Interfaces;
using Infrastructure.Context;
using DotNetEcosystemStudy.Infrastructure;

namespace DotNetEcosystemStudy.Endpoints;

public class CreateOrganization
{
    private readonly IOrganizationRepository _organizationRepository;

    public CreateOrganization(IOrganizationRepository organizationRepository)
    {
        _organizationRepository = organizationRepository;
    }

    public async Task<IResult> ActionAsync(string name, int contributorsCount)
    {
        var organization = Organization.OrganizationFactory(name, contributorsCount);
        var project = Project.ProjectFactory("Initial Project", "Description of the initial project", 5, 100, 1000);
        organization.AddProject(project);

        Console.WriteLine($"[Before CreateAsync] - [Domain] - Creating Organization with Domain Identifier: {organization.Identifier}");
        Console.WriteLine($"[Before CreateAsync] - [Domain] - Creating Project with Domain Identifier: {project.Identifier}");
        await _organizationRepository.CreateAsync(organization);
        Console.WriteLine($"[After CreateAsync] - [Domain] - Creating organization with Domain Identifier: {organization.Identifier}");
        Console.WriteLine($"[After CreateAsync] - [Domain] - Creating Project with Domain Identifier: {project.Identifier}");

        return TypedResults.Ok(new OrganizationDataModel(organization));
    }
}
