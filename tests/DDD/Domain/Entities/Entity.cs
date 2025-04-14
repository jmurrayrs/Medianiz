using Medianiz.Tests.DDD.Domain.Interfaces;
using Mediator.Interfaces;

namespace Medianiz.Tests.DDD.Domain.Entities
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();
        private List<INotification> _domainEvents = new();
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

        public void AddDomainEvent(INotification eventItem) => _domainEvents.Add(eventItem);
        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}