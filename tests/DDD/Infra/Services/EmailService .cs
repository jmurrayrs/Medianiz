using Medianiz.Tests.DDD.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Medianiz.Tests.DDD.Infra.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }

        public async Task SendOrderConfirmation(Guid orderId)
        {
            _logger.LogInformation($"Enviando email de confirmação para o pedido {orderId}");
            await Task.Delay(100);
        }
    }
}