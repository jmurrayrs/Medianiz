using Medianiz.Tests.DDD.Domain.Events;
using Medianiz.Tests.DDD.Domain.Interfaces;
using Mediator;
using Mediator.Interfaces;

namespace Medianiz.Tests.DDD.Application.Commands
{
    public class MarkOrderAsPaidCommandHandler : IRequestHandler<MarkOrderAsPaidCommand, Unit>
    {
        private readonly IOrderRepository _repository;
        private readonly IMedianiz _mediator;

        public MarkOrderAsPaidCommandHandler(IOrderRepository repository, IMedianiz mediator)
        {
            _repository = repository;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(MarkOrderAsPaidCommand request, CancellationToken cancellationToken)
        {
            var order = await _repository.GetByIdAsync(request.OrderId);
            order.MarkAsPaid();

            await _mediator.Publish(new OrderPaidEvent(order.Id), cancellationToken);

            await _repository.UpdateAsync(order);


            return Unit.Value;
        }
    }
}