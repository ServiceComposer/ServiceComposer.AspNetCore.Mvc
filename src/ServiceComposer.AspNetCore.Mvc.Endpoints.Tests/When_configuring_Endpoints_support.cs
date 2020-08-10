using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceComposer.AspNetCore.Testing;
using Xunit;

namespace ServiceComposer.AspNetCore.Mvc.Endpoints.Tests
{
    public class When_configuring_Endpoints
    {
        [Fact(Skip = "Need to find a better way to detect if there is a misconfiguration.")]
        public void Should_fail_if_no_mvc_is_configured()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                // Arrange
                var factory = new SelfContainedWebApplicationFactoryWithWebHost<When_configuring_Endpoints>
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
            var factory = new SelfContainedWebApplicationFactoryWithWebHost<When_configuring_Endpoints>
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
    }
}