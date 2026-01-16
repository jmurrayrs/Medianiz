using System.Threading;
using System.Threading.Tasks;
using Mediator.Interfaces;

namespace Medianiz.Interfaces
{
    public interface IPipelineBehavior
    {
        public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();

        public interface IPipelineBehavior<in TRequest, TResponse>
            where TRequest : IRequest<TResponse>
        {
            Task<TResponse> Handle(
                TRequest request,
                RequestHandlerDelegate<TResponse> next,
                CancellationToken cancellationToken
            );
        }

    }
}