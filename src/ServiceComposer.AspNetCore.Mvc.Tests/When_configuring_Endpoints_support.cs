using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceComposer.AspNetCore.Testing;
using System;
using System.Linq;
using Xunit;

namespace ServiceComposer.AspNetCore.Mvc.Tests
{
    public class When_configuring_Endpoints
    {
#if NETCOREAPP3_1
        [Fact]
        public void Should_fail_if_no_mvc_is_configured()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                // Arrange
                var factory = new SelfContainedWebApplicationFactoryWithWebHost<When_configuring_Mvc_support>
                (
                    configureServices: services =>
                    {
                        services.AddRouting();
                        services.AddViewModelComposition(options => { options.AddMvcSupport(); });
                    },
                    configure: app =>
                    {
                        app.UseRouting();
                        app.UseEndpoints(builder => { });
                    }
                );

                // Act
                factory.CreateClient();
            });
        }

        [Fact]
        public void Make_sure_filter_is_added_to_mvc()
        {
            MvcOptions mvcOptions = null;

            // Arrange
            var factory = new SelfContainedWebApplicationFactoryWithWebHost<When_configuring_Mvc_support>
            (
                configureServices: services =>
                {
                    services.AddRouting();
                    services.AddControllers(options => mvcOptions = options);
                    services.AddViewModelComposition(options => { options.AddMvcSupport(); });
                },
                configure: app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(builder => builder.MapControllers());
                }
            );

            // Act
            factory.CreateClient();

            // Assert
            var registeredFilter = mvcOptions
                .Filters
                .SingleOrDefault(f =>
                {
                    return f is TypeFilterAttribute tfa
                           && tfa.ImplementationType == typeof(CompositionActionFilter);
                });

            Assert.NotNull(registeredFilter);
        }
#endif
    }
}