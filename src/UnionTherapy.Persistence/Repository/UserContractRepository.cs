using Microsoft.EntityFrameworkCore;
using UnionTherapy.Application.Repository;
using UnionTherapy.Domain.Entities;
using UnionTherapy.Persistence.Context;

namespace UnionTherapy.Persistence.Repository;

public class UserContractRepository : BaseRepository<UserContract>, IUserContractRepository
{
    public UserContractRepository(BaseDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<UserContract>> GetContractsByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await GetListAsync(
            predicate: uc => uc.UserId == userId,
            include: q => q.Include(uc => uc.User)
                          .Include(uc => uc.Contract),
            orderBy: q => q.OrderByDescending(uc => uc.CreatedAt),
            cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<UserContract>> GetUsersByContractAsync(Guid contractId, CancellationToken cancellationToken = default)
    {
        return await GetListAsync(
            predicate: uc => uc.ContractId == contractId,
            include: q => q.Include(uc => uc.User)
                          .Include(uc => uc.Contract),
            orderBy: q => q.OrderByDescending(uc => uc.CreatedAt),
            cancellationToken: cancellationToken);
    }

    public async Task<UserContract?> GetUserContractAsync(Guid userId, Guid contractId, CancellationToken cancellationToken = default)
    {
        return await GetAsync(
            predicate: uc => uc.UserId == userId && uc.ContractId == contractId,
            include: q => q.Include(uc => uc.User)
                          .Include(uc => uc.Contract),
            cancellationToken: cancellationToken);
    }

    public async Task<bool> HasUserAcceptedContractAsync(Guid userId, Guid contractId, CancellationToken cancellationToken = default)
    {
        return await ExistsAsync(uc => uc.UserId == userId && uc.ContractId == contractId && uc.IsAccepted, cancellationToken);
    }
} 