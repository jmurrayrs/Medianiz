using Medianiz.Tests.DDD.Domain.Events;
using Mediator.Interfaces;
using Microsoft.Extensions.Logging;

namespace Medianiz.Tests.DDD.Application.Events
{
    public class OrderCreatedEventHandler : INotificationHandler<OrderCreatedEvent>
    {
        private readonly ILogger<OrderCreatedEventHandler> _logger;

        public OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Order created: {notification.OrderId}, Number: {notification.Number}");
            return Task.CompletedTask;
        }
    }
}