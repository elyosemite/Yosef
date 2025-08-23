namespace ProjectManagement.Domain.Events
{
    using System.Threading.Tasks;
    using Yosef.ProjectManagement.Domain.Events;

    public interface IDomainEventDispatcher
    {
        Task DispatchEventAsync(IEnumerable<IHasDomainEvent> domainEvents);
    }
}