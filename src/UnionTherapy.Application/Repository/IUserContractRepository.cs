using UnionTherapy.Domain.Entities;

namespace UnionTherapy.Application.Repository;

public interface IUserContractRepository : IBaseRepository<UserContract>
{
    Task<IEnumerable<UserContract>> GetContractsByUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserContract>> GetUsersByContractAsync(Guid contractId, CancellationToken cancellationToken = default);
    Task<UserContract?> GetUserContractAsync(Guid userId, Guid contractId, CancellationToken cancellationToken = default);
    Task<bool> HasUserAcceptedContractAsync(Guid userId, Guid contractId, CancellationToken cancellationToken = default);
} 