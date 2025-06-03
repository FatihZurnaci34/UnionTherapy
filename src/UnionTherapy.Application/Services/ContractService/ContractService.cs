using AutoMapper;
using UnionTherapy.Application.Models.Contract.Request;
using UnionTherapy.Application.Models.Contract.Response;
using UnionTherapy.Application.Repository;
using UnionTherapy.Domain.Entities;
using UnionTherapy.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace UnionTherapy.Application.Services.ContractService;

public class ContractService : IContractService
{
    private readonly IContractRepository _contractRepository;
    private readonly IUserContractRepository _userContractRepository;
    private readonly IMapper _mapper;

    public ContractService(IContractRepository contractRepository, IUserContractRepository userContractRepository, IMapper mapper)
    {
        _contractRepository = contractRepository;
        _userContractRepository = userContractRepository;
        _mapper = mapper;
    }

    public async Task<ContractGetByIdResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var contract = await _contractRepository.GetAsync(
            predicate: c => c.Id == id,
            include: q => q.Include(c => c.Psychologist!)
                          .ThenInclude(p => p.User),
            cancellationToken: cancellationToken);

        return contract != null ? _mapper.Map<ContractGetByIdResponse>(contract) : null;
    }

    public async Task<IEnumerable<ContractGetByIdResponse>> GetActiveContractsAsync(CancellationToken cancellationToken = default)
    {
        var contracts = await _contractRepository.GetListAsync(
            predicate: c => c.Status == ContractStatus.Active,
            include: q => q.Include(c => c.Psychologist!)
                          .ThenInclude(p => p.User),
            orderBy: q => q.OrderBy(c => c.Title),
            cancellationToken: cancellationToken);

        return _mapper.Map<IEnumerable<ContractGetByIdResponse>>(contracts);
    }

    public async Task<IEnumerable<ContractGetByIdResponse>> GetContractsByTypeAsync(string type, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<ContractType>(type, true, out var contractType))
            return new List<ContractGetByIdResponse>();

        var contracts = await _contractRepository.GetListAsync(
            predicate: c => c.Type == contractType,
            include: q => q.Include(c => c.Psychologist!)
                          .ThenInclude(p => p.User),
            orderBy: q => q.OrderBy(c => c.Title),
            cancellationToken: cancellationToken);

        return _mapper.Map<IEnumerable<ContractGetByIdResponse>>(contracts);
    }

    public async Task<ContractGetByIdResponse?> GetLatestContractByTypeAsync(string type, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<ContractType>(type, true, out var contractType))
            return null;

        var contract = await _contractRepository.GetAsync(
            predicate: c => c.Type == contractType && c.Status == ContractStatus.Active,
            include: q => q.Include(c => c.Psychologist!)
                          .ThenInclude(p => p.User),
            cancellationToken: cancellationToken);

        return contract != null ? _mapper.Map<ContractGetByIdResponse>(contract) : null;
    }

    public async Task<CreateContractResponse> CreateAsync(CreateContractRequest request, CancellationToken cancellationToken = default)
    {
        var contract = _mapper.Map<Contract>(request);
        contract.CreatedAt = DateTime.UtcNow;
        contract.Status = ContractStatus.Draft;

        var createdContract = await _contractRepository.AddAsync(contract, cancellationToken);
        await _contractRepository.SaveChangesAsync(cancellationToken);

        var contractResponse = _mapper.Map<ContractGetByIdResponse>(createdContract);
        return new CreateContractResponse { Contract = contractResponse };
    }

    public async Task<ContractGetByIdResponse?> UpdateAsync(Guid id, CreateContractRequest request, CancellationToken cancellationToken = default)
    {
        var existingContract = await _contractRepository.GetByIdAsync(id, cancellationToken);
        if (existingContract == null)
            return null;

        _mapper.Map(request, existingContract);
        existingContract.UpdatedAt = DateTime.UtcNow;

        var updatedContract = await _contractRepository.UpdateAsync(existingContract, cancellationToken);
        await _contractRepository.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ContractGetByIdResponse>(updatedContract);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _contractRepository.DeleteAsync(id, cancellationToken);
        await _contractRepository.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> SignContractAsync(Guid contractId, Guid userId, CancellationToken cancellationToken = default)
    {
        var contract = await _contractRepository.GetByIdAsync(contractId, cancellationToken);
        if (contract == null || contract.Status != ContractStatus.Active)
            return false;

        var existingUserContract = await _userContractRepository.GetAsync(
            predicate: uc => uc.ContractId == contractId && uc.UserId == userId,
            cancellationToken: cancellationToken);

        if (existingUserContract != null)
            return false; // Already signed

        var userContract = new UserContract
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ContractId = contractId,
            SignedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        await _userContractRepository.AddAsync(userContract, cancellationToken);
        await _userContractRepository.SaveChangesAsync(cancellationToken);
        return true;
    }
}