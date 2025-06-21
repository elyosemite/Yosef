using DotNetEcosystemStudy.DataModel;
using DotNetEcosystemStudy.Aggregates;

namespace DotNetEcosystemStudy.Endpoints;

public class GetOrganization
{
    public static IResult Action()
    {
        var organization = Organization.OrganizationFactory("DotNetEcosystemStudy", 100);

        return TypedResults.Ok(new OrganizationDataModel(organization));
    }
}
