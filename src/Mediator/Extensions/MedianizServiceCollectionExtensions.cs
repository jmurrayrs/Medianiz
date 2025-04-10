using System;
using System.Linq;
using System.Reflection;
using Mediator.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Mediator.Extensions
{
    public static class MedianizServiceCollectionExtensions
    {
        /// <summary>
        /// Registers all request and notification handlers from the specified assemblies and configures Medianiz services
        /// </summary>
        /// <param name="services">The service collection to add to</param>
        /// <param name="assemblyMarkers">Types whose assemblies should be scanned for handlers</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddMedianiz(this IServiceCollection services, params Type[] assemblyMarkers)
        {
            var assemblies = assemblyMarkers.Select(t => t.Assembly).ToArray();


            services.RegisterGenericHandlers(typeof(IRequestHandler<,>), assemblies);


            services.RegisterGenericHandlers(typeof(INotificationHandler<>), assemblies);

            services.AddScoped<IMedianiz, Medianiz>();
            return services;
        }

        /// <summary>
        /// Scans assemblies and registers all implementations of the specified handler interface type
        /// </summary>
        /// <param name="services">The service collection to register with</param>
        /// <param name="genericHandlerType">The open generic handler interface type (IRequestHandler<,> or INotificationHandler<>)</param>
        /// <param name="assemblies">Assemblies to scan for handler implementations</param>
        private static void RegisterGenericHandlers(
            this IServiceCollection services,
            Type genericHandlerType,
            Assembly[] assemblies)
        {
            var handlerTypes = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => t.GetInterfaces()
                    .Any(i =>
                        i.IsGenericType &&
                        i.GetGenericTypeDefinition() == genericHandlerType))
                .ToList();

            handlerTypes
                .SelectMany(handlerType =>
                    handlerType.GetInterfaces()
                        .Where(i =>
                            i.IsGenericType &&
                            i.GetGenericTypeDefinition() == genericHandlerType)
                        .Select(@interface => new ServiceDescriptor(@interface, handlerType, ServiceLifetime.Transient)))
                .ToList()
                .ForEach(sd => services.Add(sd));
        }
    }
}