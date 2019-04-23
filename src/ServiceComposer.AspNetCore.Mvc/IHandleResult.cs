using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace ServiceComposer.AspNetCore.Mvc
{
    public interface IHandleResult : IInterceptRoutes
    {
        Task Handle(string requestId, ResultExecutingContext context, dynamic viewModel, int httpStatusCode);
    }
}
