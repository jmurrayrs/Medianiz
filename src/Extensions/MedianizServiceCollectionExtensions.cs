using System;
using System.Linq;
using System.Reflection;
using Mediator.Interfaces;
using Mediator.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Mediator.Extensions
{
    /// <summary>
    /// Provides extension methods for registering Medianiz services with dependency injection
    /// </summary>
    public static class MedianizServiceCollectionExtensions
    {
        /// <summary>
        /// Registers Medianiz and all handlers from the specified assembly marker types
        /// </summary>
        /// <param name="services">The service collection to register with</param>
        /// <param name="assemblyMarkers">Types from assemblies to scan for handlers</param>
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
        /// Registers Medianiz with fluent configuration options
        /// </summary>
        /// <param name="services">The service collection to register with</param>
        /// <param name="configure">Configuration action for Medianiz registration options</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddMedianiz(
            this IServiceCollection services,
            Action<MedianizRegistrationOptions> configure)
        {
            var options = new MedianizRegistrationOptions();
            configure(options);

            // Obter os tipos genéricos uma vez
            Type requestHandlerType = typeof(IRequestHandler<,>);
            Type notificationHandlerType = typeof(INotificationHandler<>);

            // Registrar handlers dos assemblies
            options.Assemblies
                .Distinct()
                .ToList()
                .ForEach(assembly =>
                {
                    RegisterGenericHandlers(services, requestHandlerType, assembly, options.Lifetime);
                    RegisterGenericHandlers(services, notificationHandlerType, assembly, options.Lifetime);
                });

            // Registrar handlers explícitos
            options.ExplicitHandlers
                .Distinct()
                .ToList()
                .ForEach(handlerType =>
                    RegisterHandlerInterfaces(services, handlerType, options.Lifetime));

            return services.AddScoped<IMedianiz, Medianiz>();
        }

        /// <summary>
        /// Registers Medianiz and all handlers from a specified assembly by name
        /// </summary>
        /// <param name="services">The service collection to register with</param>
        /// <param name="assemblyName">The name of the assembly to load and scan</param>
        /// <returns>The service collection for chaining</returns>

        public static IServiceCollection AddMedianizFromAssembly(this IServiceCollection services, string assemblyName)
        {
            var assembly = AppDomain.CurrentDomain.Load(assemblyName);
            return services.AddMedianiz(assembly.GetTypes()[0]);
        }

        /// <summary>
        /// Registers Medianiz and all handlers from multiple assemblies by name
        /// </summary>
        /// <param name="services">The service collection to register with</param>
        /// <param name="assemblyNames">The names of the assemblies to load and scan</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddMedianizFromAssemblies(
            this IServiceCollection services,
            params string[] assemblyNames)
        {
            var assemblies = assemblyNames
                .Select(Assembly.Load)
                .ToArray();

            var markerTypes = assemblies
                .SelectMany(a => a.GetExportedTypes())
                .Where(t => t.IsClass)
                .Take(1)
                .ToArray();

            return services.AddMedianiz(markerTypes);
        }

        /// <summary>
        /// Registers Medianiz with custom lifetime for handlers from specified assembly markers
        /// </summary>
        /// <param name="services">The service collection to register with</param>
        /// <param name="lifetime">The service lifetime for registered handlers</param>
        /// <param name="assemblyMarkers">Types from assemblies to scan for handlers</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddMedianizWithLifetime(
            this IServiceCollection services,
            ServiceLifetime lifetime,
            params Type[] assemblyMarkers)
        {
            var assemblies = assemblyMarkers
                .Select(t => t.Assembly)
                .ToArray();

            RegisterHandlersWithLifetime(services, assemblies, lifetime);

            return services.AddScoped<IMedianiz, Medianiz>();
        }

        /// <summary>
        /// Registers all implementations of a handler interface type from multiple assemblies with custom lifetime
        /// </summary>
        /// <param name="services">The service collection to register with</param>
        /// <param name="assemblies">Assemblies to scan for handler implementations</param>
        /// <param name="lifetime">The service lifetime for registered handlers</param>
        private static void RegisterHandlersWithLifetime(
            IServiceCollection services,
            Assembly[] assemblies,
            ServiceLifetime lifetime)
        {
            // Para IRequestHandler<,>
            var requestHandlerType = typeof(IRequestHandler<,>);
            RegisterGenericHandlersWithLifetime(services, assemblies, requestHandlerType, lifetime);

            // Para INotificationHandler<>
            var notificationHandlerType = typeof(INotificationHandler<>);
            RegisterGenericHandlersWithLifetime(services, assemblies, notificationHandlerType, lifetime);
        }

        /// <summary>
        /// Registers implementations of a specific handler interface type from assemblies with custom lifetime
        /// </summary>
        /// <param name="services">The service collection to register with</param>
        /// <param name="assemblies">Assemblies to scan for handler implementations</param>
        /// <param name="genericHandlerType">The open generic handler interface type to register</param>
        /// <param name="lifetime">The service lifetime for registered handlers</param>

        private static void RegisterGenericHandlersWithLifetime(
            IServiceCollection services,
            Assembly[] assemblies,
            Type genericHandlerType,
            ServiceLifetime lifetime)
        {
            var handlerTypes = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract)
                .SelectMany(t => t.GetInterfaces()
                    .Where(i => i.IsGenericType &&
                            i.GetGenericTypeDefinition() == genericHandlerType)
                    .Select(i => new ServiceDescriptor(i, t, lifetime)))
                .ToList();

            handlerTypes.ForEach(sd => services.Add(sd));
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

        ///<summary>
        ///Registers handler implementations of a specific interface type from multiple assemblies with custom lifetime
        ///</summary>
        ///<param name="services">The service collection to register with</param>
        ///<param name="genericHandlerType">The open generic handler interface type to register</param>
        ///<param name="assembly">The assembly to scan for handler implementations</param>
        ///<param name="lifetime">The service lifetime for the registered handlers</param>
        private static void RegisterGenericHandlers(
            IServiceCollection services,
            Type genericHandlerType,
            Assembly assembly,
            ServiceLifetime lifetime)
        {
            var handlerTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .SelectMany(t => t.GetInterfaces()
                    .Where(i => i.IsGenericType &&
                            i.GetGenericTypeDefinition() == genericHandlerType)
                    .Select(i => new ServiceDescriptor(i, t, lifetime)))
                .ToList();

            handlerTypes.ForEach(sd => services.Add(sd));
        }

        /// <summary>
        /// Registers a specific handler type for all implemented handler interfaces with custom lifetime
        /// </summary>
        /// <param name="services">The service collection to register with</param>
        /// <param name="handlerType">The concrete handler type to register</param>
        /// <param name="lifetime">The service lifetime for the registered handler</param>
        private static void RegisterHandlerInterfaces(
        IServiceCollection services,
        Type handlerType,
        ServiceLifetime lifetime)
        {
            var handlerInterfaces = handlerType.GetInterfaces()
                .Where(i => i.IsGenericType &&
                        (i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>) ||
                        i.GetGenericTypeDefinition() == typeof(INotificationHandler<>)))
                .ToList();

            handlerInterfaces.ForEach(handler => services.Add(new ServiceDescriptor(handler, handlerType, lifetime)));

        }
    }

}