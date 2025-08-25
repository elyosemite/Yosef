namespace Yosef.ProjectManagement.Domain.Events;

public class OrganizationCreatedEvent : DomainEventBase
{
    private readonly Guid _organizationId;
    private readonly string _name;
    private readonly string _description;
    private readonly DateTime _createdAt;

    public Guid OrganizationId => _organizationId;
    public string Name => _name;
    public string Description => _description;
    public DateTime CreatedAt => _createdAt;

    public OrganizationCreatedEvent(
        Guid OrganizationId,
        string Name,
        string Description,
        DateTime CreatedAt)
    {
        _organizationId = OrganizationId;
        _name = Name;
        _description = Description;
        _createdAt = CreatedAt;
    }
}