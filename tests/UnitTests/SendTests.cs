using Mediator.Extensions;
using Mediator.Interfaces;
using Microsoft.Extensions.DependencyInjection;

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
    }
}
