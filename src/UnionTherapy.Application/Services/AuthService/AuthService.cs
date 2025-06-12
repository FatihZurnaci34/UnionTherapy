using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using UnionTherapy.Application.Models.Auth.Request;
using UnionTherapy.Application.Models.Auth.Response;
using UnionTherapy.Application.Repository;
using UnionTherapy.Domain.Entities;
using UnionTherapy.Application.Interfaces;
using UnionTherapy.Application.Utilities;
using UnionTherapy.Application.Exceptions;

namespace UnionTherapy.Application.Services.AuthService
{
    public class AuthService : IAuthService
    {

        IUserRepository _userRepository;
        IMapper _mapper;
        IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(IMapper mapper, IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<LoginResponse> Login(LoginRequest request, CancellationToken cancellationToken = default)
        {
            User? user = await _userRepository.GetAsync(x => x.Email == request.Email);

            if (user == null)
                throw new BusinessException("Giriş bilgileri hatalı");

            // Şifre kontrolü
            if (!HashingHelper.VerifyPasswordWithBCrypt(request.Password, user.PasswordHash))
                throw new BusinessException("Giriş bilgileri hatalı");

            // JWT token üret
            string accessToken = _jwtTokenGenerator.GenerateAccessToken(user);
            string refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

            // Refresh token'ı veritabanına kaydet
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            user.LastLoginDate = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);

            LoginResponse mappedResponse = _mapper.Map<LoginResponse>(user);
            mappedResponse.AccessToken = accessToken;
            mappedResponse.RefreshToken = refreshToken;
            mappedResponse.TokenExpiration = DateTime.UtcNow.AddHours(1);

            return mappedResponse;
        }

        public async Task Register(RegisterRequest request, CancellationToken cancellationToken = default)
        {
            // Email kontrolü
            var existingUser = await _userRepository.GetAsync(x => x.Email == request.Email);
            if (existingUser != null)
                throw new BusinessException("Bu email adresi zaten kullanılıyor");

            string hashedPassword = HashingHelper.HashPasswordWithBCrypt(request.Password);

            // AutoMapper ile RegisterRequest'ten User'a mapping
            var user = _mapper.Map<User>(request);

            user.PasswordHash = hashedPassword;

            await _userRepository.AddAsync(user);

            return;
        }


        public async Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest request, CancellationToken cancellationToken = default)
        {
            // 1. Süresi dolmuş access token'dan kullanıcı bilgilerini al
            var principal = _jwtTokenGenerator.GetPrincipalFromExpiredToken(request.AccessToken);
            var userIdClaim = principal?.FindFirst("userId")?.Value;
            
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
                throw new ValidationException("Geçersiz token formatı");

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new NotFoundException("Kullanıcı", userId);

            // Refresh token'ı doğrula
            if (user.RefreshToken != request.RefreshToken)
                throw new BusinessException("Geçersiz refresh token");

            if (user.RefreshTokenExpiry == null || user.RefreshTokenExpiry < DateTime.UtcNow)
                throw new BusinessException("Refresh token süresi dolmuş");

            // 4. Yeni tokenlar üret
            var newAccessToken = _jwtTokenGenerator.GenerateAccessToken(user);
            var newRefreshToken = _jwtTokenGenerator.GenerateRefreshToken();

            // 5. Yeni refresh token'ı veritabanına kaydet
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _userRepository.UpdateAsync(user);

            return new RefreshTokenResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                TokenExpiration = DateTime.UtcNow.AddHours(1)
            };
        }
    }
}
