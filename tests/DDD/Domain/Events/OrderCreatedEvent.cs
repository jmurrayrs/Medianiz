using Medianiz.Tests.DDD.Domain.Interfaces;

namespace Medianiz.Tests.DDD.Domain.Events
{
    public record OrderCreatedEvent(Guid OrderId, string Number, decimal Total) : IDomainEvent;

}