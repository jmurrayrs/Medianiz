using Mediator.Interfaces;

namespace Medianiz.Tests.UnitTests.Shared
{
    public class TestRequestHandler : IRequestHandler<TestRequest, string>
    {
        public Task<string> Handle(TestRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult($"Handled: {request.Message}");
        }
    }
}