using Medianiz.Tests.DDD.Application.Commands;
using Medianiz.Tests.DDD.Application.Events;
using Medianiz.Tests.DDD.Domain.Interfaces;
using Medianiz.Tests.DDD.Infra.Repositories;
using Medianiz.Tests.DDD.Infra.Services;
using Mediator;
using Mediator.Extensions;
using Mediator.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;


namespace Medianiz.Tests.UnitTests
{
    public class DDDTests
    {
        [Fact]
        public async Task Full_DDD_Workflow_With_All_Implementations()
        {
            // Arrange
            var services = new ServiceCollection();


            services.AddLogging();

            // Registering Assemblies
            services.AddMedianiz(typeof(CreateOrderHandler), typeof(OrderPaidEventHandler));


            var mockLogger = new Mock<ILogger<OrderPaidEventHandler>>();

            services.AddSingleton<IOrderRepository, OrderRepository>();
            services.AddSingleton<IEmailService, EmailService>();

            var provider = services.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMedianiz>();
            var logger = provider.GetRequiredService<ILogger<EmailService>>();

            // Act - Order Creation
            var orderId = await mediator.Send(new CreateOrderCommand("ORD-123", 100.50m));

            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains($"Order {orderId} paid")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);

            // Assert - Validation
            var repo = provider.GetRequiredService<IOrderRepository>();
            var createdOrder = await repo.GetByIdAsync(orderId);
            Assert.Equal("ORD-123", createdOrder.Number);

            // Act - Mark as paid
            var result = await mediator.Send(new MarkOrderAsPaidCommand(orderId));
            Assert.Equal(Unit.Value, result);

            // Assert - Verify payment
            var paidOrder = await repo.GetByIdAsync(orderId);
            Assert.True(paidOrder.IsPaid);
        }
    }
}