using Medianiz.Tests.DDD.Domain.Interfaces;

namespace Medianiz.Tests.DDD.Domain.Events
{
    public record OrderPaidEvent(Guid OrderId) : IDomainEvent;

}