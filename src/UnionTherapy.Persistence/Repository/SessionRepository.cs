using UnionTherapy.Application.Repository;
using UnionTherapy.Domain.Entities;
using UnionTherapy.Persistence.Context;

namespace UnionTherapy.Persistence.Repository;

public class SessionRepository : BaseRepository<Session>, ISessionRepository
{
    public SessionRepository(BaseDbContext context) : base(context)
    {
    }
} 