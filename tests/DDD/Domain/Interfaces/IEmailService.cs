namespace Medianiz.Tests.DDD.Domain.Interfaces
{
    public interface IEmailService
    {
        Task SendOrderConfirmation(Guid orderId);
    }
}