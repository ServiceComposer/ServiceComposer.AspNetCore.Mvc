using Microsoft.Extensions.DependencyInjection;
using System;

namespace ServiceComposer.AspNetCore.Mvc
{
    public class ViewModelCompositionMvcOptions
    {
        private IServiceCollection services;

        internal ViewModelCompositionMvcOptions(IServiceCollection services)
        {
            this.services = services;
        }

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
