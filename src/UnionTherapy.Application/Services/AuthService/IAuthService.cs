using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnionTherapy.Application.Models.Auth.Request;
using UnionTherapy.Application.Models.Auth.Response;


namespace UnionTherapy.Application.Services.AuthService;

public interface IAuthService
{
    Task Register(RegisterRequest request, CancellationToken cancellationToken = default);

    Task<LoginResponse> Login(LoginRequest request, CancellationToken cancellationToken = default);

    Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest request, CancellationToken cancellationToken = default);
}
