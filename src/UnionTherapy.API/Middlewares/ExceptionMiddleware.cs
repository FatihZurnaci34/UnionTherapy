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
                // Hassas bilgileri loglamaktan kaçın
                var logMessage = ex switch
                {
                    LocalizedBusinessException => "Business logic exception occurred",
                    LocalizedValidationException => "Validation exception occurred", 
                    LocalizedNotFoundException => "Resource not found exception occurred",
                    _ => "An unhandled exception occurred"
                };
                
                _logger.LogError(ex, "{LogMessage} - Path: {Path}", logMessage, context.Request.Path);
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
                    response.Message = localizationService.GetLocalizedString(ex.MessageKey, ex.Parameters ?? Array.Empty<object>());
                    break;

                case LocalizedValidationException ex:
                    response.Message = localizationService.GetLocalizedString(ex.MessageKey, ex.Parameters ?? Array.Empty<object>());
                    break;

                case LocalizedNotFoundException ex:
                    response.Message = localizationService.GetLocalizedString(ex.MessageKey, ex.Parameters ?? Array.Empty<object>());
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