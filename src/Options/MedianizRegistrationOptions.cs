using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Mediator.Options
{
    public class MedianizRegistrationOptions
    {
        public List<Assembly> Assemblies { get; } = new();
        public List<Type> ExplicitHandlers { get; } = new();
        public ServiceLifetime Lifetime { get; set; } = ServiceLifetime.Transient;

        public MedianizRegistrationOptions AddAssembly(Assembly assembly)
        {
            Assemblies.Add(assembly);
            return this;
        }

        public MedianizRegistrationOptions RegisterHandlersFromAssembly(Type assemblyMarker)
        {
            if (assemblyMarker is null)
            {
                throw new ArgumentNullException(nameof(assemblyMarker));
            }

            return AddAssembly(assemblyMarker.Assembly);
        }

        public MedianizRegistrationOptions RegisterHandlersFromAssembly(Assembly assembly)
        {
            if (assembly is null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            return AddAssembly(assembly);
        }

        public MedianizRegistrationOptions AddHandler<THandler>() where THandler : class
        {
            ExplicitHandlers.Add(typeof(THandler));
            return this;
        }

        public MedianizRegistrationOptions RegisterHandler<THandler>() where THandler : class
        {
            return AddHandler<THandler>();
        }

        public MedianizRegistrationOptions SetLifetime(ServiceLifetime lifetime)
        {
            Lifetime = lifetime;
            return this;
        }
    }
}