using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UnionTherapy.Infrastructure.Utility.JWT;
using UnionTherapy.Application.Utilities;

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
            // Auth gerektirmeyen endpoint'ler
            if (IsPublicEndpoint(context.Request.Path))
            {
                await _next(context);
                return;
            }

            var token = ExtractTokenFromHeader(context.Request);
            
            if (string.IsNullOrEmpty(token))
                throw new UnauthorizedAccessException("Token bulunamadı");

            var principal = ValidateToken(token);
            
            if (principal == null)
                throw new UnauthorizedAccessException("Geçersiz token");

            // Token geçerli, kullanıcı bilgilerini context'e ekle
            context.User = principal;

            await _next(context);
        }

        private ClaimsPrincipal? ValidateToken(string token)
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

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                // JWT token tipini kontrol et
                if (validatedToken is not JwtSecurityToken jwtToken || 
                    !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Geçersiz token algoritması");
                }

                return principal;
            }
            catch (SecurityTokenExpiredException)
            {
                throw new UnauthorizedAccessException("Token süresi dolmuş");
            }
            catch (SecurityTokenException ex)
            {
                throw new UnauthorizedAccessException($"Token güvenlik hatası: {ex.Message}");
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
                "/swagger/index.html",
                "/swagger/v1/swagger.json",
                "/openapi/v1.json",
                "/health",
                "/"
            };

            return publicPaths.Any(p => path.StartsWithSegments(p, StringComparison.OrdinalIgnoreCase));
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