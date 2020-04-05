using Microsoft.AspNetCore.Builder;

namespace Example.Api.Middlewares
{
    public static class MiddlewareExtension
    {
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMiddleware<CustomHeaderMiddleware>();
        }
    }
}
