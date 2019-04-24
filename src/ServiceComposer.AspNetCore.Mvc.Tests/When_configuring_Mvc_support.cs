using ServiceComposer.AspNetCore.Gateway;
using Xunit;
using ServiceComposer.AspNetCore.Testing;
using System;

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
                        //services.AddMvc();
                        services.AddViewModelComposition(options =>
                        {
                            options.AddMvcSupport();
                        });
                    },
                    configure: app =>
                    {
                        app.RunCompositionGatewayWithDefaultRoutes();
                    }
                );
                factory.CreateClient();
            });
        }
    }
}
