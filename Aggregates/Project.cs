namespace DotNetEcosystemStudy.Aggregates;

public class Project : IEntity<Guid>
{
    public Guid Identifier { get; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int StarsCount { get; private set; }
    public int ForksCount { get; private set; }
    public int ContributorsCount { get; private set; }
    public Guid OrganizationIdentifier { get; private set; }

    private Project(string name, string description, int starsCount, int forksCount, int contributorsCount)
    {
        Identifier = Guid.NewGuid();
        Name = name;
        Description = description;
        StarsCount = starsCount;
        ForksCount = forksCount;
        ContributorsCount = contributorsCount;
    }

    public static Project ProjectFactory(string name, string description, int starsCount, int forksCount, int contributorsCount)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Project name cannot be empty.", nameof(name));
        if (starsCount < 0)
            throw new ArgumentOutOfRangeException(nameof(starsCount), "Stars count cannot be negative.");
        if (forksCount < 0)
            throw new ArgumentOutOfRangeException(nameof(forksCount), "Forks count cannot be negative.");
        if (contributorsCount < 0)
            throw new ArgumentOutOfRangeException(nameof(contributorsCount), "Contributors count cannot be negative.");

        return new Project(name, description, starsCount, forksCount, contributorsCount);
    }

    public void UpdateOrganizationIdentifier(Guid organizationIdentifier)
    {
        if (organizationIdentifier == Guid.Empty)
            throw new ArgumentException("Organization identifier cannot be empty.", nameof(organizationIdentifier));

        OrganizationIdentifier = organizationIdentifier;
    }
}