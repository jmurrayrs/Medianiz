using Medianiz.Tests.UnitTests.Shared;
using Mediator.Extensions;
using Mediator.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using static Medianiz.Interfaces.IPipelineBehavior;

namespace Medianiz.Tests.UnitTests
{
    // Testes para Send
    public class Ping : IRequest<string> { }
    public class PingHandler : IRequestHandler<Ping, string>
    {
        public Task<string> Handle(Ping request, CancellationToken cancellationToken)
            => Task.FromResult("Pong");
    }

    public class SendTests
    {
        [Fact]
        public async Task Send_ValidRequest_ReturnsResponse()
        {
            var services = new ServiceCollection();
            services.AddMedianiz(typeof(PingHandler));
            var provider = services.BuildServiceProvider();

            var mediator = provider.GetRequiredService<IMedianiz>();
            var result = await mediator.Send(new Ping());

            Assert.Equal("Pong", result);
        }
        [Fact]
        public async Task Send_Should_Invoke_Handler()
        {
            var services = new ServiceCollection();
            services.AddMedianiz(typeof(TestRequestHandler));
            var provider = services.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMedianiz>();

            var result = await mediator.Send(new TestRequest("Hello"));

            Assert.Equal("Handled: Hello", result);
        }

        [Fact]
        public async Task Send_Should_Invoke_Behavior()
        {
            var called = false;

            var services = new ServiceCollection();
            services.AddMedianiz(typeof(TestRequestHandler));
            services.AddScoped<IPipelineBehavior<TestRequest, string>>(_ => new TestBehavior(() => called = true));
            var provider = services.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMedianiz>();

            var result = await mediator.Send(new TestRequest("Hello"));

            Assert.True(called);
            Assert.Equal("Handled: Hello", result);
        }
    }




}
