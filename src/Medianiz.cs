using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Mediator
{

    /// <summary>
    /// Mediator implementation that enables in-process messaging using CQRS patterns
    /// </summary>
    public class Medianiz : IMedianiz
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the Medianiz mediator
        /// </summary>
        /// <param name="serviceProvider">The service provider for resolving handlers</param>
        public Medianiz(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        ///<inheritdoc/>
        public async Task<TResponse> Send<TResponse>(
            IRequest<TResponse> request,
            CancellationToken cancellationToken = default
        )
        {
            var requestType = request.GetType();
            var responseType = typeof(TResponse);
            var handlerInterfaceType = typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType);
            var handler = _serviceProvider.GetService(handlerInterfaceType);

            if (handler == null)
            {
                throw new InvalidOperationException($"Handler not found for {requestType.Name}");
            }

            // Método 1: Busca na implementação concreta
            var method = handler.GetType().GetMethod("Handle", new[] { requestType, typeof(CancellationToken) });

            // Método 2: Busca na interface (para implementações explícitas)
            if (method == null)
            {
                method = handlerInterfaceType.GetMethod("Handle", new[] { requestType });
            }

            if (method == null)
            {
                throw new InvalidOperationException($"Handle method not found on {handler.GetType().Name}");
            }

            try
            {
                var result = method.Invoke(handler, new object[] { request, cancellationToken });
                return await (Task<TResponse>)result;
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException ?? ex;
            }
        }

        ///<inheritdoc/>
        public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
            where TNotification : INotification
        {
            var handlerInterfaceType = typeof(INotificationHandler<>).MakeGenericType(typeof(TNotification));
            var handlers = _serviceProvider.GetServices(handlerInterfaceType).ToList();

            var tasks = handlers.Select(handler =>
            {

                var method = handler.GetType().GetMethod("Handle",
                    new[] { typeof(TNotification), typeof(CancellationToken) });


                method ??= handler.GetType().GetMethod("Handle", new[] { typeof(TNotification) });

                if (method == null)
                {
                    throw new InvalidOperationException(
                        $"Handle method not found on {handler.GetType().Name}. ");
                }

                var parameters = method.GetParameters().Length == 1
                    ? new object[] { notification }
                    : new object[] { notification, cancellationToken };

                return (Task)method.Invoke(handler, parameters);
            }).ToArray();

            await Task.WhenAll(tasks);
        }
    }
}
