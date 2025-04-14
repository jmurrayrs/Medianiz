using Mediator.Interfaces;

namespace Medianiz.Tests.DDD.Domain.Events
{
    public record OrderCreatedEvent(Guid OrderId, string Number, decimal Total) : INotification { }

}