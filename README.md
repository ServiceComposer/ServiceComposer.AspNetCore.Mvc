<img src="assets/ServiceComposer.png" width="100" />

# ServiceComposer

ServiceComposer is a ViewModel Composition Gateway, for more details and the philosophy behind a Composition Gateway refer to the [ViewModel Composition series](https://milestone.topics.it/categories/view-model-composition) of article available on [milestone.topics.it](https://milestone.topics.it/)

 ## Usage

Create a new ASP.Net Core Mvc application and change the `ConfigureServices` method of the `Startup` as follows:

 ```csharp
 namespace WebApp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddViewModelComposition(options=>
            {
                options.AddMvcSupport();
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            /* moitted for clarity */
        }
    }
}
 ```

By adding Mvc support the Composition Gateway engine will be hosted in the Mvc application, and composition will happen on top of existing controllers. Define one or more classes implementing either the `IHandleRequests` or the `ISubscribeToCompositionEvents` based on your needs.

> Make sure the assemblies containing requests handlers and events subscribers are available to the Mvc application. By adding a reference or by simply dropping assemblies in the `bin` directory.

More details on how to implement `IHandleRequests` and `ISubscribeToCompositionEvents` are available in the following articles:

- [ViewModel Composition: show me the code!](https://milestone.topics.it/view-model-composition/2019/03/06/viewmodel-composition-show-me-the-code.html)
- [The ViewModels Lists Composition Dance](https://milestone.topics.it/view-model-composition/2019/03/21/the-viewmodels-lists-composition-dance.html)

A complete sample of a Mvc application hosting the Composition Gateway is available in the [ASP.Net Mvc Core](https://github.com/mauroservienti/designing-a-ui-for-microservices-demos/tree/master/ASP.Net Mvc Core) folder of the [Designing a UI for microservices demos](https://github.com/mauroservienti/designing-a-ui-for-microservices-demos/) project.

### Icon

[API](‪https://thenounproject.com/term/api/883169‬) by [Guilherme Simoes](https://thenounproject.com/uberux/) from [the Noun Project](https://thenounproject.com/).
