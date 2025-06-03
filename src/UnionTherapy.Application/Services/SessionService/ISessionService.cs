using UnionTherapy.Application.Models.Session.Request;
using UnionTherapy.Application.Models.Session.Response;

namespace UnionTherapy.Application.Services.SessionService;

public interface ISessionService
{
    Task<SessionGetByIdResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SessionGetListResponse> GetSessionsByPsychologistAsync(Guid psychologistId, CancellationToken cancellationToken = default);
    Task<SessionGetListResponse> GetSessionsByUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<SessionGetListResponse> GetUpcomingSessionsAsync(CancellationToken cancellationToken = default);
    Task<SessionGetListResponse> GetSessionsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<SessionGetByIdResponse?> GetSessionWithParticipantsAsync(Guid sessionId, CancellationToken cancellationToken = default);
    Task<SessionGetListResponse> GetActiveSessionsAsync(CancellationToken cancellationToken = default);
    
    Task<CreateSessionResponse> CreateAsync(CreateSessionRequest request, CancellationToken cancellationToken = default);
    Task<SessionGetByIdResponse?> UpdateAsync(Guid id, UpdateSessionRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
} 