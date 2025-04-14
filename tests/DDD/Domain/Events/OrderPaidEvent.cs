using Medianiz.Tests.DDD.Domain.Interfaces;
using Mediator.Interfaces;

namespace Medianiz.Tests.DDD.Domain.Events
{
    public record OrderPaidEvent(Guid OrderId) : INotification { }

}