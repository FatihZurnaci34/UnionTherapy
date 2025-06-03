using UnionTherapy.Application.Models.User.Request;
using UnionTherapy.Application.Models.User.Response;

namespace UnionTherapy.Application.Services.UserService;

public interface IUserService
{
    Task<UserGetByIdResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<UserGetByIdResponse?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<UserGetByIdResponse?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default);
    Task<bool> IsEmailExistsAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> IsPhoneNumberExistsAsync(string phoneNumber, CancellationToken cancellationToken = default);
    Task<UserGetListResponse> GetAllAsync(CancellationToken cancellationToken = default);
    Task<UserGetListResponse> GetUsersByRoleAsync(string role, CancellationToken cancellationToken = default);
    Task<UserGetByIdResponse?> GetUserWithPsychologistAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<UserGetListResponse> GetActiveUsersAsync(CancellationToken cancellationToken = default);
    
    Task<CreateUserResponse> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken = default);
    Task<UserGetByIdResponse?> UpdateAsync(Guid id, UpdateUserRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
} 