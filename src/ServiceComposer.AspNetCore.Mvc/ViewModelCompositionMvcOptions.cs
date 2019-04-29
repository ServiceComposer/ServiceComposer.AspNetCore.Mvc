using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace ServiceComposer.AspNetCore.Mvc
{
    public class ViewModelCompositionMvcOptions
    {
        private ViewModelCompositionOptions compositionOptions;

        internal ViewModelCompositionMvcOptions(ViewModelCompositionOptions compositionOptions)
        {
            this.compositionOptions = compositionOptions;
        }

        public void RegisterResultHandler<T>()
            where T : IHandleResult
        {
            RegisterResultHandler(typeof(T));
        }

        public void RegisterResultHandler(Type type)
        {
            compositionOptions.Services.AddSingleton(typeof(IHandleResult), type);
        }

        internal void Initialize()
        {
            if (compositionOptions.AssemblyScanner.IsEnabled)
            {
                compositionOptions.AddTypesRegistrationHandler(
                    typesFilter: type =>
                    {
                        var typeInfo = type.GetTypeInfo();
                        return !typeInfo.IsInterface
                            && !typeInfo.IsAbstract
                            && typeof(IHandleResult).IsAssignableFrom(type);
                    },
                    registrationHandler: types =>
                    {
                        foreach (var type in types)
                        {
                            RegisterResultHandler(type);
                        }
                    });
            }

            compositionOptions.Services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(typeof(CompositionActionFilter));
            });
        }
    }
}
