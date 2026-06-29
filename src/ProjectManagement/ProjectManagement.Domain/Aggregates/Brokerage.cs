using Yosef.ProjectManagement.Domain.Events;

namespace ProjectManagement.Domain.Aggregates;

public class Brokerage : EntityBase<Guid>, IAggregateRoot<Guid>, ITableObject<int>
{
    public string Name { get; private set; }
    public bool Active { get; private set; }
    public string CNPJ { get; private set; }
    public string Email { get; private set; }
    public string? Phone { get; private set; }
    private List<Project> _projects = new();
    public IReadOnlyCollection<Project> Projects => _projects.AsReadOnly();

    public int Id { get; private set; }

    private Brokerage() { }

    private Brokerage(string name, string cnpj, string email, string? phone)
    {
        Identifier = Guid.NewGuid();
        Name = name;
        CNPJ = cnpj;
        Email = email;
        Phone = phone;
        Active = true;

        RegisterDomainEvent(new BrokerageCreatedEvent(Identifier, Name, "Brokerage Created Event", DateTime.UtcNow));
    }

    private Brokerage(Guid identifier, string name, string cnpj, string email, string? phone)
    {
        Identifier = identifier;
        Name = name;
        CNPJ = cnpj;
        Email = email;
        Phone = phone;
        Active = true;
    }

    public static Brokerage BrokerageFactory(string name, string cnpj, string email, string? phone = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Brokerage name cannot be empty.", nameof(name));
        if (string.IsNullOrWhiteSpace(cnpj))
            throw new ArgumentException("CNPJ cannot be empty.", nameof(cnpj));
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty.", nameof(email));

        return new Brokerage(name, cnpj, email, phone);
    }

    public static Brokerage BrokerageFactory(Guid identifier, string name, string cnpj, string email, string? phone = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Brokerage name cannot be empty.", nameof(name));
        if (string.IsNullOrWhiteSpace(cnpj))
            throw new ArgumentException("CNPJ cannot be empty.", nameof(cnpj));
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty.", nameof(email));
        if (identifier == Guid.Empty)
            throw new ArgumentException("Identifier cannot be empty.", nameof(identifier));

        return new Brokerage(identifier, name, cnpj, email, phone);
    }

    public void ArchiveBrokerage()
    {
        if (!Active)
            throw new InvalidOperationException("Brokerage is already archived.");

        Active = false;
        RegisterDomainEvent(new BrokerageArchivedEvent(Identifier, DateTime.UtcNow));
    }

    public void AddProject(Project project)
    {
        if (project == null)
            throw new ArgumentNullException(nameof(project), "Project cannot be null.");

        if (Identifier == Guid.Empty)
            throw new InvalidOperationException("BrokerageIdentifier must be set before adding a project.");

        project.UpdateOrganizationIdentifier(Identifier);

        _projects.Add(project);
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Brokerage name cannot be empty.", nameof(name));

        Name = name;
        RegisterDomainEvent(new BrokerageNameUpdatedEvent(Identifier, Name));
    }

    public void UpdateTableRegisterId(int id)
    {
        Id = id;
    }

    #region Event Sourcing
    public void ApplyEvent(BrokerageNameUpdatedEvent @event)
    {
        if (@event == null)
            throw new ArgumentNullException(nameof(@event), "Event cannot be null.");

        if (Identifier != @event.BrokerageId)
            throw new InvalidOperationException("Event BrokerageId does not match this Brokerage's Identifier.");

        Name = @event.BrokerageName;
    }

    public void ApplyEvent(BrokerageCreatedEvent @event)
    {
        if (@event == null)
            throw new ArgumentNullException(nameof(@event), "Event cannot be null.");

        if (Identifier != @event.BrokerageId)
            throw new InvalidOperationException("Event BrokerageId does not match this Brokerage's Identifier.");

        Name = @event.Name;
    }

    public void ApplyEvent(BrokerageArchivedEvent @event)
    {
        if (@event == null)
            throw new ArgumentNullException(nameof(@event), "Event cannot be null.");

        if (Identifier != @event.BrokerageId)
            throw new InvalidOperationException("Event BrokerageId does not match this Brokerage's Identifier.");

        Active = @event.Active;
    }

    public void Evolve(DomainEventBase domainEvent)
    {
        switch (domainEvent)
        {
            case BrokerageNameUpdatedEvent e:
                ApplyEvent(e);
                break;
            case BrokerageCreatedEvent e:
                ApplyEvent(e);
                break;
            case BrokerageArchivedEvent e:
                ApplyEvent(e);
                break;
            default:
                throw new ArgumentException($"Unsupported event type: {domainEvent.GetType().Name}", nameof(domainEvent));
        }
    }

    public static Brokerage Rehydrate(IEnumerable<DomainEventBase> events)
    {
        var brokerage = new Brokerage();
        foreach (var e in events)
        {
            brokerage.ApplyEvent((dynamic)e);
        }
        return brokerage;
    }
    #endregion
}
