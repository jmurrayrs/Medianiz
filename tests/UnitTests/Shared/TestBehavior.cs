using Mediator.Interfaces;
using static Medianiz.Interfaces.IPipelineBehavior;

namespace Medianiz.Tests.UnitTests.Shared
{
    public record TestRequest(string Message) : IRequest<string>;

    public class TestBehavior : IPipelineBehavior<TestRequest, string>
    {
        private readonly Action _action;

        public TestBehavior(Action action)
        {
            _action = action;
        }

        public async Task<string> Handle(
            TestRequest request,
            RequestHandlerDelegate<string> next,
            CancellationToken cancellationToken)
        {
            _action();
            return await next();
        }
    }

}