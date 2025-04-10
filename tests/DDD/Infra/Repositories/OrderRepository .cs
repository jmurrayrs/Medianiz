using Medianiz.Tests.DDD.Domain.Entities;
using Medianiz.Tests.DDD.Domain.Interfaces;

namespace Medianiz.Tests.DDD.Infra.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly List<Order> _orders = new();

        public Task<Order> GetByIdAsync(Guid id)
        {
            var order = _orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return default!;
            }
            return Task.FromResult(order);
        }

        public Task AddAsync(Order order)
        {
            _orders.Add(order);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Order order)
        {
            var index = _orders.FindIndex(o => o.Id == order.Id);
            if (index >= 0)
            {
                _orders[index] = order;
            }
            return Task.CompletedTask;
        }
    }
}