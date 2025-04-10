using Medianiz.Tests.DDD.Domain.Entities;

namespace Medianiz.Tests.DDD.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> GetByIdAsync(Guid id);
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
    }
}