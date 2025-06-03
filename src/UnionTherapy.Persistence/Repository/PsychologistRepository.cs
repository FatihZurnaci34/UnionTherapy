using UnionTherapy.Application.Repository;
using UnionTherapy.Domain.Entities;
using UnionTherapy.Persistence.Context;

namespace UnionTherapy.Persistence.Repository;

public class PsychologistRepository : BaseRepository<Psychologist>, IPsychologistRepository
{
    public PsychologistRepository(BaseDbContext context) : base(context)
    {
    }
} 