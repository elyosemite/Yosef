using Yosef.ProjectManagement.Domain.Events;

namespace ProjectManagement.Domain.Aggregates;

public class Organization : EntityBase<Guid>, IAggregateRoot<Guid>, ITableObject<int>
{
    public string Name { get; private set; }
    public bool Active { get; private set; }
    public int ContributorsCount { get; private set; }
    public string? Secret { get; private set; }
    private List<Project> _projects = new();
    public IReadOnlyCollection<Project> Projects => _projects.AsReadOnly();

    public int Id { get; private set; }

    private Organization() { }

    private Organization(string name, int contributorsCount, string? secret = null)
    {
        Identifier = Guid.NewGuid();
        Name = name;
        ContributorsCount = contributorsCount;
        Secret = secret;
        Active = true;

        RegisterDomainEvent(new OrganizationCreatedEvent(Identifier, Name, "Organization Created Event", DateTime.UtcNow));
    }

    private Organization(Guid identifier, string name, int contributorsCount, string? secret = null)
    {
        Identifier = identifier;
        Name = name;
        ContributorsCount = contributorsCount;
        Secret = secret;
        Active = true;
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

    public void UpdateSecret(string secret) => Secret = secret;
    public void ArchiveOrganization()
    {
        if (!Active)
            throw new InvalidOperationException("Organization is already archived.");

        Active = false;
        RegisterDomainEvent(new OrganizationArchivedEvent(Identifier, DateTime.UtcNow));
    }

    public void AddProject(Project project)
    {
        if (project == null)
            throw new ArgumentNullException(nameof(project), "Project cannot be null.");

        if (Identifier == Guid.Empty)
            throw new InvalidOperationException("OrganizationIdentifier must be set before adding a project.");

        project.UpdateOrganizationIdentifier(Identifier);

        _projects.Add(project);
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Organization name cannot be empty.", nameof(name));

        Name = name;
        RegisterDomainEvent(new OrgNameUpdatedEvent(Identifier, Name));
    }

    public void UpdateTableRegisterId(int id)
    {
        Id = id;
    }

    #region Implement Event Sourcing
    public void ApplyEvent(OrgNameUpdatedEvent orgNameUpdatedEvent)
    {
        if (orgNameUpdatedEvent == null)
            throw new ArgumentNullException(nameof(orgNameUpdatedEvent), "Event cannot be null.");

        if (Identifier != orgNameUpdatedEvent.OrganizationId)
            throw new InvalidOperationException("Event OrganizationId does not match this Organization's Identifier.");

        Name = orgNameUpdatedEvent.OrganizationName;
    }

    public void ApplyEvent(OrganizationCreatedEvent organizationCreatedEvent)
    {
        if (organizationCreatedEvent == null)
            throw new ArgumentNullException(nameof(organizationCreatedEvent), "Event cannot be null.");

        if (Identifier != organizationCreatedEvent.OrganizationId)
            throw new InvalidOperationException("Event OrganizationId does not match this Organization's Identifier.");

        Name = organizationCreatedEvent.Name;
    }

    public void ApplyEvent(OrganizationArchivedEvent organizationArchivedEvent)
    {
        if (organizationArchivedEvent == null)
            throw new ArgumentNullException(nameof(organizationArchivedEvent), "Event cannot be null.");

        if (Identifier != organizationArchivedEvent.OrganizationId)
            throw new InvalidOperationException("Event OrganizationId does not match this Organization's Identifier.");

        Active = organizationArchivedEvent.Active;
    }

    public void Evolve(DomainEventBase domainEvent)
    {
        switch (domainEvent)
        {
            case OrgNameUpdatedEvent orgNameUpdatedEvent:
                ApplyEvent(orgNameUpdatedEvent);
                break;
            case OrganizationCreatedEvent organizationCreatedEvent:
                ApplyEvent(organizationCreatedEvent);
                break;
            case OrganizationArchivedEvent organizationArchivedEvent:
                ApplyEvent(organizationArchivedEvent);
                break;
            default:
                throw new ArgumentException($"Unsupported event type: {domainEvent.GetType().Name}", nameof(domainEvent));
        }
    }

    // Rehydrate aggregate from a sequence of events
    public static Organization Rehydrate(IEnumerable<DomainEventBase> events)
    {
        var org = new Organization();
        foreach (var e in events)
        {
            org.ApplyEvent((dynamic)e);
        }
        return org;
    }
    #endregion
}
