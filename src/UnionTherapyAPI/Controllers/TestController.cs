using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UnionTherapy.Application.Utilities;

namespace UnionTherapyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet("public")]
        public ActionResult PublicEndpoint()
        {
            var data = new { 
                message = "Bu endpoint herkese açık", 
                timestamp = DateTime.UtcNow 
            };
            
            return Ok(ResponseHelper.Success(data));
        }

        [HttpGet("protected")]
        public ActionResult ProtectedEndpoint()
        {
            // Middleware tarafından doğrulanan kullanıcı bilgileri
            var userId = User.FindFirst("userId")?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var name = User.FindFirst(ClaimTypes.Name)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            var data = new { 
                message = "Bu endpoint korumalı - JWT token gerekli",
                user = new {
                    id = userId,
                    email = email,
                    name = name,
                    role = role
                },
                timestamp = DateTime.UtcNow 
            };

            return Ok(ResponseHelper.Success(data));
        }

        [HttpGet("admin")]
        public ActionResult AdminEndpoint()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            
            if (role != "Admin")
            {
                throw new UnauthorizedAccessException("Bu endpoint sadece admin kullanıcılar için");
            }

            var data = new { 
                message = "Admin endpoint'ine hoş geldiniz",
                timestamp = DateTime.UtcNow 
            };

            return Ok(ResponseHelper.Success(data));
        }
    }
} 