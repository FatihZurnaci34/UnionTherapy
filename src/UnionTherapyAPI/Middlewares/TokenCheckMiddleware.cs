using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UnionTherapy.Infrastructure.Utility.JWT;

namespace UnionTherapyAPI.Middlewares
{
    public class TokenCheckMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TokenCheckMiddleware> _logger;
        private readonly JwtOptions _jwtOptions;

        public TokenCheckMiddleware(
            RequestDelegate next, 
            ILogger<TokenCheckMiddleware> logger,
            IOptions<JwtOptions> jwtOptions)
        {
            _next = next;
            _logger = logger;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Auth gerektirmeyen endpoint'ler
                if (IsPublicEndpoint(context.Request.Path))
                {
                    await _next(context);
                    return;
                }

                var token = ExtractTokenFromHeader(context.Request);
                
                if (string.IsNullOrEmpty(token))
                {
                    await HandleUnauthorized(context, "Token bulunamadı");
                    return;
                }

                var principal = ValidateToken(token);
                
                if (principal == null)
                {
                    await HandleUnauthorized(context, "Geçersiz token");
                    return;
                }

                // Token geçerli, kullanıcı bilgilerini context'e ekle
                context.User = principal;

                await _next(context);
            }
            catch (Exception ex)
            {
                // Sadece beklenmeyen hatalar loglanır
                _logger.LogError(ex, "Middleware kritik hatası");
                await HandleUnauthorized(context, "Sistem hatası");
            }
        }

        private ClaimsPrincipal? ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtOptions.SecretKey);

                // Token formatını ve imzasını doğrula
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _jwtOptions.Issuer,
                    ValidateAudience = true,
                    ValidAudience = _jwtOptions.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                // JWT token tipini kontrol et
                if (validatedToken is not JwtSecurityToken jwtToken || 
                    !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return null; // Geçersiz algoritma
                }

                return principal; // ✅ Geçerli token
            }
            catch (SecurityTokenExpiredException)
            {
                return null; // Token süresi dolmuş
            }
            catch (SecurityTokenException)
            {
                return null; // Token güvenlik hatası
            }
            catch (Exception ex)
            {
                // Sadece beklenmeyen hatalar loglanır
                _logger.LogError(ex, "Token doğrulama sırasında beklenmeyen hata");
                return null;
            }
        }

        private string? ExtractTokenFromHeader(HttpRequest request)
        {
            var authHeader = request.Headers["Authorization"].FirstOrDefault();
            
            if (string.IsNullOrEmpty(authHeader))
                return null;

            if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                return authHeader.Substring("Bearer ".Length).Trim();

            return null;
        }

        private bool IsPublicEndpoint(PathString path)
        {
            var publicPaths = new[]
            {
                "/api/auth/login",
                "/api/auth/register", 
                "/api/auth/refresh",
                "/api/test/public",
                "/weatherforecast",
                "/swagger",
                "/health"
            };

            return publicPaths.Any(p => path.StartsWithSegments(p, StringComparison.OrdinalIgnoreCase));
        }

        private async Task HandleUnauthorized(HttpContext context, string message)
        {
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";
            
            var response = new { message = message, timestamp = DateTime.UtcNow };
            await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
        }
    }

    // Extension method for easier registration
    public static class TokenCheckMiddlewareExtensions
    {
        public static IApplicationBuilder UseTokenCheck(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TokenCheckMiddleware>();
        }
    }
} 