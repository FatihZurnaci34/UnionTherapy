using UnionTherapy.Application.Models.Psychologist.Request;
using UnionTherapy.Application.Models.Psychologist.Response;

namespace UnionTherapy.Application.Services.PsychologistService;

public interface IPsychologistService
{
    Task<PsychologistGetByIdResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PsychologistGetByIdResponse?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<PsychologistGetListResponse> GetApprovedPsychologistsAsync(CancellationToken cancellationToken = default);
    Task<PsychologistGetListResponse> GetPendingApprovalsAsync(CancellationToken cancellationToken = default);
    Task<PsychologistGetListResponse> GetActivePsychologistsAsync(CancellationToken cancellationToken = default);
    Task<PsychologistGetByIdResponse?> GetPsychologistWithSessionsAsync(Guid psychologistId, CancellationToken cancellationToken = default);
    
    Task<CreatePsychologistResponse> CreateAsync(CreatePsychologistRequest request, CancellationToken cancellationToken = default);
    Task<PsychologistGetByIdResponse?> UpdateAsync(Guid id, CreatePsychologistRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ApproveAsync(Guid id, string approvalNote, CancellationToken cancellationToken = default);
} 