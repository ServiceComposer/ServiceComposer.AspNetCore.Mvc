using Microsoft.Extensions.DependencyInjection;
using System;

namespace ServiceComposer.AspNetCore.Mvc
{
    public class ViewModelCompositionMvcOptions
    {
        private IServiceCollection services;

        internal ViewModelCompositionMvcOptions(IServiceCollection services, bool isAssemblyScanningDisabled)
        {
            this.services = services;
            IsAssemblyScanningDisabled = isAssemblyScanningDisabled;
        }

        public bool IsAssemblyScanningDisabled { get; private set; }

        public void RegisterResultHandler<T>()
            where T : IHandleResult
        {
            RegisterResultHandler(typeof(T));
        }

        public void RegisterResultHandler(Type type)
        {
            services.AddSingleton(typeof(IHandleResult), type);
        }
    }
}
