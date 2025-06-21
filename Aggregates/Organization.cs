namespace DotNetEcosystemStudy.Aggregates;

public class Organization : IAggregateRoot<Guid>
{
    public string Name { get; private set; }
    public int ContributorsCount { get; private set; }

    /// <summary>
    /// This field is just used in Domain Model, not in Data Model.
    /// </summary>
    public string? Secret { get; private set; }
    //public int Id { get; private set; }
    public Guid Identifier { get; private set; }
    private List<Project> _projects = new();
    public IReadOnlyCollection<Project> Projects => _projects.AsReadOnly();

    private Organization(string name, int contributorsCount, string? secret = null)
    {
        Identifier = Guid.NewGuid();
        Name = name;
        ContributorsCount = contributorsCount;
        Secret = secret;
    }

    public static Organization OrganizationFactory(string name, int contributorsCount)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Organization name cannot be empty.", nameof(name));
        if (contributorsCount < 0)
            throw new ArgumentOutOfRangeException(nameof(contributorsCount), "Contributors count cannot be negative.");

        return new Organization(name, contributorsCount);
    }

    public void UpdateSecret(string secret)
        => Secret = secret;

    public void AddProject(Project project)
    {
        if (project == null)
            throw new ArgumentNullException(nameof(project), "Project cannot be null.");

        _projects.Add(project);
    }
}