namespace ProjectManagement.Domain.Aggregates;

public class Organization : IAggregateRoot<Guid>, ITableObject<int>
{
    public string Name { get; private set; }
    public int ContributorsCount { get; private set; }
    public string? Secret { get; private set; }
    public Guid Identifier { get; private set; }
    private List<Project> _projects = new();
    public IReadOnlyCollection<Project> Projects => _projects.AsReadOnly();

    public int Id { get; private set; }

    private Organization(string name, int contributorsCount, string? secret = null)
    {
        Identifier = Guid.NewGuid();
        Name = name;
        ContributorsCount = contributorsCount;
        Secret = secret;
    }

    private Organization(Guid identifier, string name, int contributorsCount, string? secret = null)
    {
        Identifier = identifier;
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

    public static Organization OrganizationFactory(Guid identifier, string name, int contributorsCount)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Organization name cannot be empty.", nameof(name));
        if (contributorsCount < 0)
            throw new ArgumentOutOfRangeException(nameof(contributorsCount), "Contributors count cannot be negative.");
        if (identifier == Guid.Empty)
            throw new ArgumentException("Identifier cannot be empty.", nameof(identifier));

        return new Organization(identifier, name, contributorsCount);
    }

    public void UpdateSecret(string secret)
        => Secret = secret;

    public void AddProject(Project project)
    {
        if (project == null)
            throw new ArgumentNullException(nameof(project), "Project cannot be null.");

        if (Identifier == Guid.Empty)
            throw new InvalidOperationException("OrganizationIdentifier must be set before adding a project.");
        
        project.UpdateOrganizationIdentifier(Identifier);

        _projects.Add(project);
    }

    public void UpdateTableRegisterId(int id)
    {
        Id = id;
    }
}