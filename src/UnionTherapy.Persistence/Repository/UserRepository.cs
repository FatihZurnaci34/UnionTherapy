using UnionTherapy.Application.Repository;
using UnionTherapy.Domain.Entities;
using UnionTherapy.Persistence.Context;

namespace UnionTherapy.Persistence.Repository;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(BaseDbContext context) : base(context)
    {
    }
} 