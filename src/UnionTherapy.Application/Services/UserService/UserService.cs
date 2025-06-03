using AutoMapper;
using UnionTherapy.Application.Models.User.Request;
using UnionTherapy.Application.Models.User.Response;
using UnionTherapy.Application.Repository;
using UnionTherapy.Domain.Entities;
using UnionTherapy.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace UnionTherapy.Application.Services.UserService;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserGetByIdResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        return user != null ? _mapper.Map<UserGetByIdResponse>(user) : null;
    }

    public async Task<UserGetByIdResponse?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetAsync(u => u.Email == email, cancellationToken: cancellationToken);
        return user != null ? _mapper.Map<UserGetByIdResponse>(user) : null;
    }

    public async Task<UserGetByIdResponse?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetAsync(u => u.PhoneNumber == phoneNumber, cancellationToken: cancellationToken);
        return user != null ? _mapper.Map<UserGetByIdResponse>(user) : null;
    }

    public async Task<bool> IsEmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _userRepository.ExistsAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<bool> IsPhoneNumberExistsAsync(string phoneNumber, CancellationToken cancellationToken = default)
    {
        return await _userRepository.ExistsAsync(u => u.PhoneNumber == phoneNumber, cancellationToken);
    }

    public async Task<UserGetListResponse> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var users = await _userRepository.GetListAsync(
            orderBy: q => q.OrderBy(u => u.FirstName).ThenBy(u => u.LastName),
            cancellationToken: cancellationToken);

        var userResponses = _mapper.Map<IEnumerable<UserGetByIdResponse>>(users);
        return new UserGetListResponse
        {
            Users = userResponses,
            TotalCount = userResponses.Count()
        };
    }

    public async Task<UserGetListResponse> GetActiveUsersAsync(CancellationToken cancellationToken = default)
    {
        var users = await _userRepository.GetListAsync(
            predicate: u => u.IsActive,
            orderBy: q => q.OrderBy(u => u.FirstName).ThenBy(u => u.LastName),
            cancellationToken: cancellationToken);

        var userResponses = _mapper.Map<IEnumerable<UserGetByIdResponse>>(users);
        return new UserGetListResponse
        {
            Users = userResponses,
            TotalCount = userResponses.Count()
        };
    }

    public async Task<UserGetListResponse> GetUsersByRoleAsync(string role, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<UserRole>(role, true, out var userRole))
            return new UserGetListResponse { Users = new List<UserGetByIdResponse>(), TotalCount = 0 };

        var users = await _userRepository.GetListAsync(
            predicate: u => u.Role == userRole,
            orderBy: q => q.OrderBy(u => u.FirstName).ThenBy(u => u.LastName),
            cancellationToken: cancellationToken);

        var userResponses = _mapper.Map<IEnumerable<UserGetByIdResponse>>(users);
        return new UserGetListResponse
        {
            Users = userResponses,
            TotalCount = userResponses.Count()
        };
    }

    public async Task<UserGetByIdResponse?> GetUserWithPsychologistAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetAsync(
            predicate: u => u.Id == userId,
            include: q => q.Include(u => u.Psychologist!),
            cancellationToken: cancellationToken);
        
        return user != null ? _mapper.Map<UserGetByIdResponse>(user) : null;
    }

    public async Task<CreateUserResponse> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        var user = _mapper.Map<User>(request);
        user.CreatedAt = DateTime.UtcNow;
        user.IsActive = true;

        var createdUser = await _userRepository.AddAsync(user, cancellationToken);
        await _userRepository.SaveChangesAsync(cancellationToken);

        var userResponse = _mapper.Map<UserGetByIdResponse>(createdUser);
        return new CreateUserResponse { User = userResponse };
    }

    public async Task<UserGetByIdResponse?> UpdateAsync(Guid id, UpdateUserRequest request, CancellationToken cancellationToken = default)
    {
        var existingUser = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (existingUser == null)
            return null;

        _mapper.Map(request, existingUser);
        existingUser.UpdatedAt = DateTime.UtcNow;

        var updatedUser = await _userRepository.UpdateAsync(existingUser, cancellationToken);
        await _userRepository.SaveChangesAsync(cancellationToken);

        return _mapper.Map<UserGetByIdResponse>(updatedUser);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _userRepository.DeleteAsync(id, cancellationToken);
        await _userRepository.SaveChangesAsync(cancellationToken);
        return true;
    }
} 