using UnionTherapy.Domain.Entities;

namespace UnionTherapy.Application.Repository;

public interface ISessionParticipationRepository : IBaseRepository<SessionParticipation>
{
    Task<IEnumerable<SessionParticipation>> GetParticipationsByUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<SessionParticipation>> GetParticipationsBySessionAsync(Guid sessionId, CancellationToken cancellationToken = default);
    Task<SessionParticipation?> GetUserSessionParticipationAsync(Guid userId, Guid sessionId, CancellationToken cancellationToken = default);
    Task<bool> IsUserParticipatingInSessionAsync(Guid userId, Guid sessionId, CancellationToken cancellationToken = default);
} 