using Medianiz.Tests.DDD.Application.Commands;
using Medianiz.Tests.DDD.Application.Events;
using Medianiz.Tests.DDD.Domain.Interfaces;
using Medianiz.Tests.DDD.Infra.Repositories;
using Medianiz.Tests.DDD.Infra.Services;
using Medianiz.Tests.UnitTests.Shared;
using Mediator.Extensions;
using Mediator.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;



namespace Medianiz.Tests.UnitTests
{
    public class DDDTests
    {

        private readonly ITestOutputHelper _output = default!;
        private readonly TestLogger<OrderPaidEventHandler> _testLogger = default!;

        public DDDTests(ITestOutputHelper output)
        {
            _output = output;
            _testLogger = new TestLogger<OrderPaidEventHandler>();
        }

        [Fact]
        public async Task Full_DDD_Workflow_With_Real_Loggers()
        {
            // Arrange
            var services = new ServiceCollection();

            // 1. Registrar todos os loggers necessários
            var orderPaidLogger = new TestLogger<OrderPaidEventHandler>();
            var emailServiceLogger = new TestLogger<EmailService>();

            services.AddSingleton<ILogger<OrderPaidEventHandler>>(orderPaidLogger);
            services.AddSingleton<ILogger<EmailService>>(emailServiceLogger);

            // 2. Implementações reais dos serviços
            services.AddSingleton<IOrderRepository, OrderRepository>();
            services.AddSingleton<IEmailService, EmailService>();

            // 3. Registrar todos os handlers necessários
            services.AddMedianiz(
                typeof(CreateOrderCommandHandler),
                typeof(MarkOrderAsPaidCommandHandler),
                typeof(OrderPaidEventHandler));

            var provider = services.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMedianiz>();
            var repo = provider.GetRequiredService<IOrderRepository>();

            // Act 1 - Criação do pedido
            var orderId = await mediator.Send(new CreateOrderCommand("ORD-123", 100.50m));

            // Assert 1 - Verificação básica
            var order = await repo.GetByIdAsync(orderId);
            Assert.NotNull(order);
            Assert.Equal("ORD-123", order.Number);
            Assert.False(order.IsPaid);

            // Act 2 - Marcar como pago (dispara OrderPaidEvent)
            await mediator.Send(new MarkOrderAsPaidCommand(orderId));

            // Assert 2 - Verificação do estado
            var paidOrder = await repo.GetByIdAsync(orderId);
            Assert.True(paidOrder?.IsPaid);

            // Assert 3 - Verificar se o OrderPaidEventHandler processou
            var expectedLog = $"Order paid: {orderId}";
            Assert.Contains(orderPaidLogger.LogEntries, x => x.Contains(expectedLog));

            // Assert 4 - Verificar se o EmailService foi chamado
            Assert.NotEmpty(emailServiceLogger.LogEntries);
        }

    }
}