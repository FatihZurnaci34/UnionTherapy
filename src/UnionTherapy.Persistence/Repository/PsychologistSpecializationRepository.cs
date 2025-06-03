using Microsoft.EntityFrameworkCore;
using UnionTherapy.Application.Repository;
using UnionTherapy.Domain.Entities;
using UnionTherapy.Domain.Enums;
using UnionTherapy.Persistence.Context;

namespace UnionTherapy.Persistence.Repository;

public class PsychologistSpecializationRepository : BaseRepository<PsychologistSpecialization>, IPsychologistSpecializationRepository
{
    public PsychologistSpecializationRepository(BaseDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<PsychologistSpecialization>> GetSpecializationsByPsychologistAsync(Guid psychologistId, CancellationToken cancellationToken = default)
    {
        return await GetListAsync(
            predicate: ps => ps.PsychologistId == psychologistId,
            include: q => q.Include(ps => ps.Psychologist)
                          .ThenInclude(p => p.User),
            cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<PsychologistSpecialization>> GetPsychologistsBySpecializationAsync(SpecializationArea specialization, CancellationToken cancellationToken = default)
    {
        return await GetListAsync(
            predicate: ps => ps.SpecializationArea == specialization,
            include: q => q.Include(ps => ps.Psychologist)
                          .ThenInclude(p => p.User),
            cancellationToken: cancellationToken);
    }

    public async Task<bool> HasPsychologistSpecializationAsync(Guid psychologistId, SpecializationArea specialization, CancellationToken cancellationToken = default)
    {
        return await ExistsAsync(ps => ps.PsychologistId == psychologistId && ps.SpecializationArea == specialization, cancellationToken);
    }
} 