using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace UnionTherapyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet("public")]
        public ActionResult<object> PublicEndpoint()
        {
            return Ok(new { 
                message = "Bu endpoint herkese açık", 
                timestamp = DateTime.UtcNow 
            });
        }

        [HttpGet("protected")]
        public ActionResult<object> ProtectedEndpoint()
        {
            // Middleware tarafından doğrulanan kullanıcı bilgileri
            var userId = User.FindFirst("userId")?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var name = User.FindFirst(ClaimTypes.Name)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new { 
                message = "Bu endpoint korumalı - JWT token gerekli",
                user = new {
                    id = userId,
                    email = email,
                    name = name,
                    role = role
                },
                timestamp = DateTime.UtcNow 
            });
        }

        [HttpGet("admin")]
        public ActionResult<object> AdminEndpoint()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            
            if (role != "Admin")
            {
                return Forbid("Bu endpoint sadece admin kullanıcılar için");
            }

            return Ok(new { 
                message = "Admin endpoint'ine hoş geldiniz",
                timestamp = DateTime.UtcNow 
            });
        }
    }
} 