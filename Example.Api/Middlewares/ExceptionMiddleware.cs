using System;
using System.Net;
using System.Threading.Tasks;
using Example.Api.Utils;
using Example.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace Example.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                Log.Error($"Exception: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            switch (exception)
            {
                case BadRequestException _:
                    context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    break;
                case ConflictException _:
                    context.Response.StatusCode = (int) HttpStatusCode.Conflict;
                    break;
                case ForbiddenException _:
                    context.Response.StatusCode = (int) HttpStatusCode.Forbidden;
                    break;
                case NotFoundException _:
                    context.Response.StatusCode = (int) HttpStatusCode.NotFound;
                    break;
                case NotImplementedException _:
                    context.Response.StatusCode = (int) HttpStatusCode.NotImplemented;
                    break;
                case UnauthorizedException _:
                    context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                    break;
                case UnprocessableEntityException _:
                    context.Response.StatusCode = (int) HttpStatusCode.UnprocessableEntity;
                    break;
                default:
                    context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    break;
            }

            return context.Response.WriteAsync(
                new ErrorResponse
                {
                    StatusCode = context.Response.StatusCode,
                    Message = exception.Message
                }.ToString()
            );
        }
    }
}
