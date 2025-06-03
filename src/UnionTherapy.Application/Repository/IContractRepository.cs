using UnionTherapy.Domain.Entities;
using UnionTherapy.Domain.Enums;

namespace UnionTherapy.Application.Repository;

public interface IContractRepository : IBaseRepository<Contract>
{
    Task<IEnumerable<Contract>> GetActiveContractsAsync(CancellationToken cancellationToken = default);
    Task<Contract?> GetContractByNameAsync(string contractName, CancellationToken cancellationToken = default);
    Task<IEnumerable<Contract>> GetContractsByTypeAsync(ContractType contractType, CancellationToken cancellationToken = default);
} 