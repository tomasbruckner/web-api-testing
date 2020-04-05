using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Example.Api.Middlewares
{
    public class CustomHeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            httpContext.Response.OnStarting(
                () =>
                {
                    httpContext.Response.Headers.Add("Cache-Control", "no-cache, no-store, max-age=0, must-revalidate");
                    return Task.FromResult(0);
                }
            );

            await _next(httpContext);
        }
    }
}
