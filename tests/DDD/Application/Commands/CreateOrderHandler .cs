using Medianiz.Tests.DDD.Domain.Entities;
using Medianiz.Tests.DDD.Domain.Interfaces;
using Medianiz.Tests.DDD.Extensions;
using Mediator.Interfaces;

namespace Medianiz.Tests.DDD.Application.Commands
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly IOrderRepository _repository;
        private readonly IMedianiz _mediator;

        public CreateOrderHandler(IOrderRepository repository, IMedianiz mediator)
        {
            _repository = repository;
            _mediator = mediator;
        }

        public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = new Order(request.Number, request.Total);
            await _repository.AddAsync(order);

            // Publica eventos de domínio
            await _mediator.PublishDomainEvents(order);
            return order.Id;
        }
    }
}