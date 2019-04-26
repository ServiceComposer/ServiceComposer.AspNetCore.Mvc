using System;
using System.Reflection;

namespace ServiceComposer.AspNetCore.Mvc
{
    public static class ViewModelCompositionOptionsExtensions
    {
        public static void AddMvcSupport(this ViewModelCompositionOptions compositionOptions)
        {
            AddMvcSupport(compositionOptions, _ => { });
        }

        public static void AddMvcSupport(this ViewModelCompositionOptions compositionOptions, Action<ViewModelCompositionMvcOptions> config)
        {
            var mvcCompositionOptions = new ViewModelCompositionMvcOptions(compositionOptions.Services);
            config?.Invoke(mvcCompositionOptions);

            if (compositionOptions.AssemblyScanner.IsEnabled)
            {
                compositionOptions.AssemblyScanner.RegisterTypeFilter(
                    filter: type =>
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
                            mvcCompositionOptions.RegisterResultHandler(type);
                        }
                    });
            }
        }
    }
}
