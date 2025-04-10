using Medianiz.Tests.DDD.Domain.Events;
using Medianiz.Tests.DDD.Domain.Interfaces;
using Mediator.Interfaces;
using Microsoft.Extensions.Logging;

namespace Medianiz.Tests.DDD.Application.Events
{
    public class OrderPaidEventHandler : INotificationHandler<OrderPaidEvent>
    {
        private readonly ILogger<OrderPaidEventHandler> _logger;
        private readonly IEmailService _emailService;

        public OrderPaidEventHandler(ILogger<OrderPaidEventHandler> logger, IEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }

        public async Task Handle(OrderPaidEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Order paid: {notification.OrderId}");
            await _emailService.SendOrderConfirmation(notification.OrderId);
        }
    }
}