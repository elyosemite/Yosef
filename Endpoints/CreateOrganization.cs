using DotNetEcosystemStudy.DataModel;
using DotNetEcosystemStudy.Aggregates;
using DotNetEcosystemStudy.Settings.Interfaces;
using Infrastructure.Context;

namespace DotNetEcosystemStudy.Endpoints;

public class CreateOrganization
{
    private readonly IGlobalSettings _globalSettings;

    public CreateOrganization(IGlobalSettings globalSettings)
    {
        _globalSettings = globalSettings;
    }

    public IResult Action(string name, int contributorsCount)
    {
        var context = new OrganizationContext(_globalSettings);
        var organization = Organization.OrganizationFactory(name, contributorsCount);
        var project = Project.ProjectFactory("Initial Project", "Description of the initial project", 5, 100, 1000);
        organization.AddProject(project);

        context.Organization.Add(organization);
        context.SaveChanges();

        return TypedResults.Ok(new OrganizationDataModel(organization));
    }
}
