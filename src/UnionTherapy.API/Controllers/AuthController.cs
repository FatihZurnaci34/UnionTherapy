using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using UnionTherapy.Application.Models.Auth.Request;
using UnionTherapy.Application.Models.Auth.Response;
using UnionTherapy.Application.Services.AuthService;
using UnionTherapy.Application.Utilities;

namespace UnionTherapyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [EnableRateLimiting("AuthPolicy")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _authService.Login(request);
            return Ok(ResponseHelper.Success(response));
        }

        [HttpPost("register")]
        [EnableRateLimiting("AuthPolicy")]
        public async Task<ActionResult> Register([FromBody] RegisterRequest request)
        {
            await _authService.Register(request);
            return Ok(ResponseHelper.Success());
        }

        [HttpPost("refresh")]
        [EnableRateLimiting("AuthPolicy")]
        public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var response = await _authService.RefreshToken(request);
            return Ok(ResponseHelper.Success(response));
        }
    }
} 