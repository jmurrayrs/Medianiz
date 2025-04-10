using Medianiz.Tests.DDD.Domain.Entities;
using Mediator.Interfaces;

namespace Medianiz.Tests.DDD.Extensions
{
    public static class MedianizExtensions
    {
        public static async Task PublishDomainEvents(this IMedianiz mediator, Entity entity)
        {
            var domainEvents = entity.DomainEvents.ToArray();
            entity.ClearDomainEvents();

            await Task.WhenAll(domainEvents.Select(e => mediator.Publish(e)));
        }
    }
}