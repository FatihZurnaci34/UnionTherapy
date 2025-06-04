using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using UnionTherapy.Application.Utilities;

namespace UnionTherapy.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            context.Response.StatusCode = StatusCodes.Status200OK; 

            var response = ResponseHelper.Fail<string>(exception.Message);

            await context.Response.WriteAsJsonAsync(response);
        }
    }
} 