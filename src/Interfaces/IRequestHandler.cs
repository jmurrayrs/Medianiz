using System.Threading;
using System.Threading.Tasks;

namespace Mediator.Interfaces
{
    /// <summary>
    /// Defines a request handler interface
    /// </summary>
    /// <typeparam name="TRequest">Request type</typeparam>
    /// <typeparam name="TResponse">Response type</typeparam>
    /// <summary>

    public interface IRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        /// <summary>
        /// Handles a request
        /// </summary>
        /// <param name="request">Request object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task containing the response</returns>
        Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }
}