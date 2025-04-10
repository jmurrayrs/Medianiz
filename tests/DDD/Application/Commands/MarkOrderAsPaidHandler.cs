using Medianiz.Tests.DDD.Domain.Interfaces;
using Medianiz.Tests.DDD.Extensions;
using Mediator;
using Mediator.Interfaces;

namespace Medianiz.Tests.DDD.Application.Commands
{
    public class MarkOrderAsPaidHandler : IRequestHandler<MarkOrderAsPaidCommand, Unit>
    {
        private readonly IOrderRepository _repository;
        private readonly IMedianiz _mediator;

        public MarkOrderAsPaidHandler(IOrderRepository repository, IMedianiz mediator)
        {
            _repository = repository;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(MarkOrderAsPaidCommand request, CancellationToken cancellationToken)
        {
            var order = await _repository.GetByIdAsync(request.OrderId);
            order.MarkAsPaid();
            await _repository.UpdateAsync(order);

            await _mediator.PublishDomainEvents(order);
            return Unit.Value;
        }
    }
}