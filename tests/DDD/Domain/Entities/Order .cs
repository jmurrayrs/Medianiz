using Medianiz.Tests.DDD.Domain.Events;

namespace Medianiz.Tests.DDD.Domain.Entities
{
    public class Order : Entity
    {
        public string Number { get; private set; }
        public decimal Total { get; private set; }
        public bool IsPaid { get; private set; }

        public Order(string number, decimal total)
        {
            Number = number;
            Total = total;
            AddDomainEvent(new OrderCreatedEvent(Id, number, total));
        }

        public void MarkAsPaid()
        {
            IsPaid = true;
            AddDomainEvent(new OrderPaidEvent(Id));
        }
    }
}