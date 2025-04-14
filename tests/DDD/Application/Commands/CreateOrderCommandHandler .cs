using Medianiz.Tests.DDD.Domain.Entities;
using Medianiz.Tests.DDD.Domain.Interfaces;
using Mediator.Interfaces;

namespace Medianiz.Tests.DDD.Application.Commands
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly IOrderRepository _repository;
        private readonly IMedianiz _mediator;

        public CreateOrderCommandHandler(IOrderRepository repository, IMedianiz mediator)
        {
            _repository = repository;
            _mediator = mediator;
        }

        public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = new Order(request.Number, request.Total);
            await _repository.AddAsync(order);            

            return order.Id;
        }       
    }
}