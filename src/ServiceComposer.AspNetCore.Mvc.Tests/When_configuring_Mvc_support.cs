using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ServiceComposer.AspNetCore.Testing;
using System;
using System.Linq;
using Xunit;

namespace ServiceComposer.AspNetCore.Mvc.Tests
{
    public class When_configuring_Mvc_support
    {
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
                        services.AddViewModelComposition(options =>
                        {
                            options.AddMvcSupport();
                        });
                    },
                    configure: app =>
                    {
                        app.UseMvc();
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
                    services.AddMvc(options => mvcOptions = options);
                    services.AddViewModelComposition(options =>
                    {
                        options.AddMvcSupport();
                    });
                },
                configure: app =>
                {
                    app.UseMvc();
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