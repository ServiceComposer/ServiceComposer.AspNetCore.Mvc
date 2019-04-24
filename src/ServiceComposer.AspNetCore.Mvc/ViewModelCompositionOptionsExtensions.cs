using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ServiceComposer.AspNetCore.Mvc
{
    public static class ViewModelCompositionOptionsExtensions
    {
        public static void AddMvcSupport(this ViewModelCompositionOptions compositionOptions, Action<ViewModelCompositionMvcOptions> config)
        {
            var mvcCompositionOptions = new ViewModelCompositionMvcOptions(
                    compositionOptions.Services, 
                    compositionOptions.IsAssemblyScanningDisabled);

            config?.Invoke(mvcCompositionOptions);
            if (!compositionOptions.IsAssemblyScanningDisabled)
            {
                var fileNames = Directory.GetFiles(AppContext.BaseDirectory);
                var types = new List<Type>();
                foreach (var fileName in fileNames)
                {
                    types.AddRange(Assembly.LoadFrom(fileName).GetTypesFromAssembly());
                }

                var platformAssembliesString = (string)AppDomain.CurrentDomain.GetData("TRUSTED_PLATFORM_ASSEMBLIES");
                if (platformAssembliesString != null)
                {
                    var platformAssemblies = platformAssembliesString.Split(Path.PathSeparator);
                    foreach (var platformAssembly in platformAssemblies)
                    {
                        types.AddRange(Assembly.LoadFrom(platformAssembly).GetTypesFromAssembly());
                    }
                }

                foreach (var type in types)
                {
                    mvcCompositionOptions.RegisterResultHandler(type);
                }
            }

            //TODO: throw if MvcOptions is not defined, it means AddMvc has not been yet called.
            compositionOptions.Services.Configure<MvcOptions>(options => 
            {
                options.Filters.Add(typeof(CompositionActionFilter));
            });
        }

        static IEnumerable<Type> GetTypesFromAssembly(this Assembly assembly)
        {
            var types = assembly.GetTypes()
                .Where(t =>
                {
                    var typeInfo = t.GetTypeInfo();
                    return !typeInfo.IsInterface
                        && !typeInfo.IsAbstract
                        && typeof(IHandleResult).IsAssignableFrom(t);
                });

            return types;
        }
    }
}
