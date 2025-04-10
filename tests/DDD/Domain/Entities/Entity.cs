using Medianiz.Tests.DDD.Domain.Interfaces;

namespace Medianiz.Tests.DDD.Domain.Entities
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();
        private List<IDomainEvent> _domainEvents = new();
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void AddDomainEvent(IDomainEvent eventItem) => _domainEvents.Add(eventItem);
        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}