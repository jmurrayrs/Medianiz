using Mediator;
using Mediator.Extensions;
using Mediator.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Medianiz.Tests.UnitTests
{
    public class MedianizServiceCollectionExtensionsTests
    {
        public class TestCommand : IRequest<string> { }
        public class TestCommandHandler : IRequestHandler<TestCommand, string>
        {
            public Task<string> Handle(TestCommand request, CancellationToken ct)
                => Task.FromResult("OK");
        }

        public class TestEvent : INotification { }
        public class TestEventHandler : INotificationHandler<TestEvent>
        {
            public Task Handle(TestEvent notification, CancellationToken ct)
                => Task.CompletedTask;
        }

        [Fact]
        public void AddMedianizWithLifetime_Should_Register_Handlers_With_Specified_Lifetime()
        {
            // Arrange
            var services = new ServiceCollection();
            var lifetime = ServiceLifetime.Singleton;

            // Act
            services.AddMedianizWithLifetime(lifetime, typeof(TestCommandHandler));

            // Assert
            var requestDescriptor = services.FirstOrDefault(x =>
                x.ServiceType == typeof(IRequestHandler<TestCommand, string>));
            var eventDescriptor = services.FirstOrDefault(x =>
                x.ServiceType == typeof(INotificationHandler<TestEvent>));

            Assert.Equal(lifetime, requestDescriptor?.Lifetime);
            Assert.Equal(lifetime, eventDescriptor?.Lifetime);
        }

        [Fact]
        public void AddMedianizWithLifetime_Should_Register_Medianiz_As_Scoped()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddMedianizWithLifetime(ServiceLifetime.Transient, typeof(TestCommandHandler));

            // Assert
            var mediatorDescriptor = services.FirstOrDefault(x =>
                x.ServiceType == typeof(IMedianiz));

            Assert.Equal(ServiceLifetime.Scoped, mediatorDescriptor?.Lifetime);
        }

        [Fact]
        public void AddMedianizWithLifetime_Should_Register_All_Handlers_In_Assembly()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddMedianizWithLifetime(
                ServiceLifetime.Transient,
                typeof(TestCommandHandler),
                typeof(TestEventHandler));

            // Assert
            Assert.Contains(services, x =>
                x.ServiceType == typeof(IRequestHandler<TestCommand, string>));

            Assert.Contains(services, x =>
                x.ServiceType == typeof(INotificationHandler<TestEvent>));
        }

        [Fact]
        public void AddMedianiz_Default_Should_Use_Transient_Lifetime_For_Handlers()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddMedianiz(typeof(TestCommandHandler));

            // Assert
            var descriptor = services.FirstOrDefault(x =>
                x.ServiceType == typeof(IRequestHandler<TestCommand, string>));

            Assert.Equal(ServiceLifetime.Transient, descriptor?.Lifetime);
        }

        [Fact]
        public void AddMedianiz_Should_Work_With_Multiple_Assemblies()
        {
            // Arrange
            var services = new ServiceCollection();
            var assembly1 = typeof(TestCommandHandler).Assembly;
            var assembly2 = typeof(AnotherAssemblyHandler).Assembly;

            // Act
            services.AddMedianiz(
                typeof(TestCommandHandler),
                typeof(AnotherAssemblyHandler));

            // Assert
            Assert.Contains(services, x =>
                x.ServiceType == typeof(IRequestHandler<TestCommand, string>));

            Assert.Contains(services, x =>
                x.ServiceType == typeof(IRequestHandler<AnotherCommand, Unit>));
        }
    }

    // Helper types for multi-assembly test
    public class AnotherCommand : IRequest<Unit> { }
    public class AnotherAssemblyHandler : IRequestHandler<AnotherCommand, Unit>
    {
        public Task<Unit> Handle(AnotherCommand request, CancellationToken ct)
            => Task.FromResult(Unit.Value);
    }
}
