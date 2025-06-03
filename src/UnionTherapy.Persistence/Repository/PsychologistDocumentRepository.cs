using Microsoft.EntityFrameworkCore;
using UnionTherapy.Application.Repository;
using UnionTherapy.Domain.Entities;
using UnionTherapy.Domain.Enums;
using UnionTherapy.Persistence.Context;

namespace UnionTherapy.Persistence.Repository;

public class PsychologistDocumentRepository : BaseRepository<PsychologistDocument>, IPsychologistDocumentRepository
{
    public PsychologistDocumentRepository(BaseDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<PsychologistDocument>> GetDocumentsByPsychologistAsync(Guid psychologistId, CancellationToken cancellationToken = default)
    {
        return await GetListAsync(
            predicate: pd => pd.PsychologistId == psychologistId,
            include: q => q.Include(pd => pd.Psychologist)
                          .ThenInclude(p => p.User),
            orderBy: q => q.OrderByDescending(pd => pd.CreatedAt),
            cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<PsychologistDocument>> GetDocumentsByTypeAsync(string documentType, CancellationToken cancellationToken = default)
    {
        return await GetListAsync(
            predicate: pd => pd.DocumentType == documentType,
            include: q => q.Include(pd => pd.Psychologist)
                          .ThenInclude(p => p.User),
            orderBy: q => q.OrderByDescending(pd => pd.CreatedAt),
            cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<PsychologistDocument>> GetVerifiedDocumentsAsync(CancellationToken cancellationToken = default)
    {
        return await GetListAsync(
            predicate: pd => pd.Status == DocumentStatus.Approved,
            include: q => q.Include(pd => pd.Psychologist)
                          .ThenInclude(p => p.User),
            orderBy: q => q.OrderByDescending(pd => pd.CreatedAt),
            cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<PsychologistDocument>> GetPendingDocumentsAsync(CancellationToken cancellationToken = default)
    {
        return await GetListAsync(
            predicate: pd => pd.Status == DocumentStatus.Pending,
            include: q => q.Include(pd => pd.Psychologist)
                          .ThenInclude(p => p.User),
            orderBy: q => q.OrderBy(pd => pd.CreatedAt),
            cancellationToken: cancellationToken);
    }
} 