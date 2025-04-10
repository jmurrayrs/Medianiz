using Mediator;
using Mediator.Interfaces;

namespace Medianiz.Tests.DDD.Application.Commands
{
    public record MarkOrderAsPaidCommand(Guid OrderId) : IRequest<Unit>;

}