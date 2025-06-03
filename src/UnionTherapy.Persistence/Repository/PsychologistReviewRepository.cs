using Microsoft.EntityFrameworkCore;
using UnionTherapy.Application.Repository;
using UnionTherapy.Domain.Entities;
using UnionTherapy.Persistence.Context;

namespace UnionTherapy.Persistence.Repository;

public class PsychologistReviewRepository : BaseRepository<PsychologistReview>, IPsychologistReviewRepository
{
    public PsychologistReviewRepository(BaseDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<PsychologistReview>> GetReviewsByPsychologistAsync(Guid psychologistId, CancellationToken cancellationToken = default)
    {
        return await GetListAsync(
            predicate: pr => pr.PsychologistId == psychologistId,
            include: q => q.Include(pr => pr.User)
                          .Include(pr => pr.Psychologist)
                          .ThenInclude(p => p.User),
            orderBy: q => q.OrderByDescending(pr => pr.CreatedAt),
            cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<PsychologistReview>> GetReviewsByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await GetListAsync(
            predicate: pr => pr.UserId == userId,
            include: q => q.Include(pr => pr.User)
                          .Include(pr => pr.Psychologist)
                          .ThenInclude(p => p.User),
            orderBy: q => q.OrderByDescending(pr => pr.CreatedAt),
            cancellationToken: cancellationToken);
    }

    public async Task<double> GetAverageRatingByPsychologistAsync(Guid psychologistId, CancellationToken cancellationToken = default)
    {
        var reviews = await GetListAsync(
            predicate: pr => pr.PsychologistId == psychologistId,
            enableTracking: false,
            cancellationToken: cancellationToken);

        return reviews.Any() ? reviews.Average(pr => pr.Rating) : 0;
    }

    public async Task<int> GetReviewCountByPsychologistAsync(Guid psychologistId, CancellationToken cancellationToken = default)
    {
        return await CountAsync(pr => pr.PsychologistId == psychologistId, cancellationToken);
    }
} 