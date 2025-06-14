using System.Threading;
using System.Threading.Tasks;

namespace Mediator.Interfaces
{
    /// <summary>
    /// Defines a notification handler interface
    /// </summary>
    /// <typeparam name="TNotification">Notification type</typeparam>
    public interface INotificationHandler<in TNotification> where TNotification : INotification
    {
        /// <summary>
        /// Handles a notification
        /// </summary>
        /// <param name="notification">Notification object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task Handle(TNotification notification, CancellationToken cancellationToken);
    }
}