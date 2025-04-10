using System.Threading;
using System.Threading.Tasks;

namespace Mediator.Interfaces
{
    public interface IMedianiz
    {
        /// <summary>
        /// Sends a request to a single handler and returns the response
        /// </summary>
        /// <typeparam name="TResponse">The type of response expected</typeparam>
        /// <param name="request">The request object</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>A task representing the asynchronous operation with the response</returns>
        Task<TResponse> Send<TResponse>(
            IRequest<TResponse> request,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Publishes a notification to all registered handlers
        /// </summary>
        /// <typeparam name="TNotification">The type of notification</typeparam>
        /// <param name="notification">The notification object</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task Publish<TNotification>(
            TNotification notification,
            CancellationToken cancellationToken = default) where TNotification : INotification;
    }

}