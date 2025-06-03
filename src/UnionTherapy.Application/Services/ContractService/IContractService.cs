using UnionTherapy.Application.Models.Contract.Request;
using UnionTherapy.Application.Models.Contract.Response;

namespace UnionTherapy.Application.Services.ContractService;

public interface IContractService
{
    Task<ContractGetByIdResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ContractGetByIdResponse>> GetActiveContractsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ContractGetByIdResponse>> GetContractsByTypeAsync(string type, CancellationToken cancellationToken = default);
    Task<ContractGetByIdResponse?> GetLatestContractByTypeAsync(string type, CancellationToken cancellationToken = default);
    
    Task<CreateContractResponse> CreateAsync(CreateContractRequest request, CancellationToken cancellationToken = default);
    Task<ContractGetByIdResponse?> UpdateAsync(Guid id, CreateContractRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> SignContractAsync(Guid contractId, Guid userId, CancellationToken cancellationToken = default);
} 