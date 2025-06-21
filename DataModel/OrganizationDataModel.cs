using DotNetEcosystemStudy.Aggregates;

namespace DotNetEcosystemStudy.DataModel;

public class OrganizationDataModel
{
    public string Name { get; set; } = string.Empty;
    public int ContributorsCount { get; set; }

    public OrganizationDataModel() {}
    public OrganizationDataModel(Organization organization)
        => (Name, ContributorsCount) = (organization.Name, organization.ContributorsCount);
}
