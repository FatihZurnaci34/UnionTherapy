using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using UnionTherapy.Application.Utilities;
using UnionTherapy.Application.Exceptions;
using System.Net;
using System.Text.Json;
using UnionTherapy.Application.Services.Localization;
using UnionTherapy.Application.Constants;

namespace UnionTherapyAPI.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            
            // Scoped service'i burada al
            var localizationService = context.RequestServices.GetRequiredService<ILocalizationService>();
            
            var response = new Response<object>
            {
                IsSuccess = false,
                Data = null
            };

            switch (exception)
            {
                case LocalizedBusinessException ex:
                    response.Message = localizationService.GetLocalizedString(ex.MessageKey, ex.Parameters);
                    break;

                case LocalizedValidationException ex:
                    response.Message = localizationService.GetLocalizedString(ex.MessageKey, ex.Parameters);
                    break;

                case LocalizedNotFoundException ex:
                    response.Message = localizationService.GetLocalizedString(ex.MessageKey, ex.Parameters);
                    break;

                case BusinessException ex:
                    response.Message = ex.Message;
                    break;

                case ValidationException ex:
                    response.Message = ex.Message;
                    break;

                case NotFoundException ex:
                    response.Message = ex.Message;
                    break;

                default:
                    response.Message = localizationService.GetLocalizedString("UnexpectedError");
                    break;
            }

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);
        }
    }
} 