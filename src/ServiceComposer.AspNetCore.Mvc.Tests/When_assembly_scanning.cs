using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using ServiceComposer.AspNetCore.Gateway;
using System.Threading.Tasks;
using Xunit;
using ServiceComposer.AspNetCore.Testing;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ServiceComposer.AspNetCore.Mvc.Tests
{
    public class When_assembly_scanning
    {
        class SampleNeverInvokedResultHandler : IHandleResult
        {
            public Task Handle(string requestId, ResultExecutingContext context, dynamic viewModel, int httpStatusCode)
            {
                throw new NotImplementedException();
            }

            public bool Matches(RouteData routeData, string httpVerb, HttpRequest request)
            {
                return false;
            }
        }

        [Fact]
        public void Should_not_fail_due_to_invalid_assemblies()
        {
            // Arrange
            var factory = new SelfContainedWebApplicationFactoryWithWebHost<When_assembly_scanning>
            (
                configureServices: services =>
                {
                    services.AddMvc();
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
        }

        [Fact]
        public void Should_not_register_duplicated_result_handlers()
        {
            IServiceProvider container = null;
            // Arrange
            var factory = new SelfContainedWebApplicationFactoryWithWebHost<When_assembly_scanning>
            (
                configureServices: services =>
                {
                    services.AddMvc();
                    services.AddViewModelComposition(options=> 
                    {
                        options.AddMvcSupport();
                    });
                },
                configure: app =>
                {
                    container= app.ApplicationServices;
                    app.RunCompositionGatewayWithDefaultRoutes();
                }
            );
            factory.CreateClient();

            var handler = container.GetServices<IHandleResult>()
                .SingleOrDefault(svc => svc is SampleNeverInvokedResultHandler);

            Assert.NotNull(handler);
        }
    }
}
