using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using UnionTherapy.Application.Utilities;
using UnionTherapy.Application.Exceptions;

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
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            // Her zaman 200 OK döndür
            context.Response.StatusCode = StatusCodes.Status200OK;

            var message = GetErrorMessage(exception);

            // Loglama - Exception türüne göre farklı seviyeler
            LogException(exception);

            // isSuccess = false ile hata response'u döndür
            var response = ResponseHelper.Fail<string>(message);

            await context.Response.WriteAsJsonAsync(response);
        }

        private string GetErrorMessage(Exception exception)
        {
            return exception switch
            {
                // Custom Application Exceptions
                ValidationException validationEx => string.Join(", ", validationEx.Errors),
                BusinessException => exception.Message,
                NotFoundException => exception.Message,
                
                // Authentication & Authorization
                UnauthorizedAccessException => exception.Message,
                
                // Critical System Exceptions (EF Core)
                DbUpdateException => "Veritabanı güncelleme hatası",
                
                // Veritabanı bağlantı hataları (PostgreSQL yok durumu için)
                InvalidOperationException when exception.Message.Contains("transient failure") => 
                    "Veritabanı bağlantı hatası",
                
                // Default - Tüm diğer hatalar
                _ => "Beklenmeyen bir hata oluştu"
            };
        }

        private void LogException(Exception exception)
        {
            switch (exception)
            {
                // Client errors - Warning level
                case ValidationException:
                case BusinessException:
                case NotFoundException:
                case UnauthorizedAccessException:
                    _logger.LogWarning("İstemci hatası: {ExceptionType} - {Message}", 
                        exception.GetType().Name, exception.Message);
                    break;
                    
                // Server errors - Error level
                case DbUpdateException:
                    _logger.LogError(exception, "Sistem hatası: {ExceptionType} - {Message}", 
                        exception.GetType().Name, exception.Message);
                    break;
                    
                // Unknown errors - Error level
                default:
                    _logger.LogError(exception, "Beklenmeyen hata: {ExceptionType} - {Message}", 
                        exception.GetType().Name, exception.Message);
                    break;
            }
        }
    }
} 