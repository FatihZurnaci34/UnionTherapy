using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UnionTherapy.Application.Utilities;
using UnionTherapy.Application.Constants;
using UnionTherapy.Application.Exceptions;

namespace UnionTherapyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet("public")]
        public IActionResult GetPublicData()
        {
            var data = new { message = "Bu public bir endpoint", timestamp = DateTime.UtcNow };
            return Ok(ResponseHelper.Success(data, ResponseMessages.OperationSuccessful));
        }

        [HttpGet("protected")]
        public IActionResult GetProtectedData()
        {
            var data = new { message = "Bu protected bir endpoint", user = "test_user", timestamp = DateTime.UtcNow };
            return Ok(ResponseHelper.Success(data, ResponseMessages.OperationSuccessful));
        }

        [HttpGet("admin")]
        public IActionResult GetAdminData()
        {
            var data = new { message = "Bu admin endpoint", permissions = new[] { "read", "write", "delete" }, timestamp = DateTime.UtcNow };
            return Ok(ResponseHelper.Success(data, ResponseMessages.OperationSuccessful));
        }

        [HttpGet("test-error")]
        public IActionResult TestError()
        {
            // Localized exception örneği
            throw new LocalizedBusinessException(ResponseMessages.InvalidToken);
        }

        [HttpGet("test-not-found")]
        public IActionResult TestNotFound()
        {
            // Localized not found exception örneği
            throw new LocalizedNotFoundException(ResponseMessages.UserNotFound);
        }

        [HttpGet("test-validation")]
        public IActionResult TestValidation()
        {
            // Localized validation exception örneği
            throw new LocalizedValidationException(ResponseMessages.ValidationFailed);
        }

        [HttpGet("test-language")]
        public IActionResult TestLanguage([FromHeader(Name = "X-Language")] string? language = "tr")
        {
            // Bu endpoint dil header'ını parametre olarak alır
            // Swagger'da X-Language header field'ı otomatik görünecek
            throw new LocalizedBusinessException(ResponseMessages.InvalidCredentials);
        }
    }
} 