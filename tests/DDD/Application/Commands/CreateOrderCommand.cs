using Mediator.Interfaces;

namespace Medianiz.Tests.DDD.Application.Commands
{
    public record CreateOrderCommand(string Number, decimal Total) : IRequest<Guid>;

}