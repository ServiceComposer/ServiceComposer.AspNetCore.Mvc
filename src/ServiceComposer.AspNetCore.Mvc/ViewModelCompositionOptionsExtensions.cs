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
            var options = new ViewModelCompositionMvcOptions(compositionOptions);
            config?.Invoke(options);

            options.Initialize();
        }
    }
}
