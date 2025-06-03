using Microsoft.EntityFrameworkCore;
using UnionTherapy.Application.Repository;
using UnionTherapy.Domain.Entities;
using UnionTherapy.Domain.Enums;
using UnionTherapy.Persistence.Context;

namespace UnionTherapy.Persistence.Repository;

public class ContractRepository : BaseRepository<Contract>, IContractRepository
{
    public ContractRepository(BaseDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Contract>> GetActiveContractsAsync(CancellationToken cancellationToken = default)
    {
        return await GetListAsync(
            predicate: c => c.Status == ContractStatus.Active,
            orderBy: q => q.OrderBy(c => c.Title),
            cancellationToken: cancellationToken);
    }

    public async Task<Contract?> GetContractByNameAsync(string contractName, CancellationToken cancellationToken = default)
    {
        return await GetAsync(c => c.Title == contractName, cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<Contract>> GetContractsByTypeAsync(ContractType contractType, CancellationToken cancellationToken = default)
    {
        return await GetListAsync(
            predicate: c => c.Type == contractType,
            orderBy: q => q.OrderBy(c => c.Title),
            cancellationToken: cancellationToken);
    }
} 