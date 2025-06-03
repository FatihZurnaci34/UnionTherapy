using Microsoft.EntityFrameworkCore;
using UnionTherapy.Application.Repository;
using UnionTherapy.Domain.Entities;
using UnionTherapy.Persistence.Context;

namespace UnionTherapy.Persistence.Repository;

public class SessionParticipationRepository : BaseRepository<SessionParticipation>, ISessionParticipationRepository
{
    public SessionParticipationRepository(BaseDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<SessionParticipation>> GetParticipationsByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await GetListAsync(
            predicate: sp => sp.UserId == userId,
            include: q => q.Include(sp => sp.User)
                          .Include(sp => sp.Session)
                          .ThenInclude(s => s.Psychologist)
                          .ThenInclude(p => p.User),
            orderBy: q => q.OrderByDescending(sp => sp.CreatedAt),
            cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<SessionParticipation>> GetParticipationsBySessionAsync(Guid sessionId, CancellationToken cancellationToken = default)
    {
        return await GetListAsync(
            predicate: sp => sp.SessionId == sessionId,
            include: q => q.Include(sp => sp.User)
                          .Include(sp => sp.Session),
            orderBy: q => q.OrderByDescending(sp => sp.CreatedAt),
            cancellationToken: cancellationToken);
    }

    public async Task<SessionParticipation?> GetUserSessionParticipationAsync(Guid userId, Guid sessionId, CancellationToken cancellationToken = default)
    {
        return await GetAsync(
            predicate: sp => sp.UserId == userId && sp.SessionId == sessionId,
            include: q => q.Include(sp => sp.User)
                          .Include(sp => sp.Session),
            cancellationToken: cancellationToken);
    }

    public async Task<bool> IsUserParticipatingInSessionAsync(Guid userId, Guid sessionId, CancellationToken cancellationToken = default)
    {
        return await ExistsAsync(sp => sp.UserId == userId && sp.SessionId == sessionId, cancellationToken);
    }
} 