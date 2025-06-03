using Microsoft.EntityFrameworkCore;
using UnionTherapy.Application.Repository;
using UnionTherapy.Domain.Entities;
using UnionTherapy.Persistence.Context;

namespace UnionTherapy.Persistence.Repository;

public class ReviewRepository : BaseRepository<Review>, IReviewRepository
{
    public ReviewRepository(BaseDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Review>> GetReviewsByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await GetListAsync(
            predicate: r => r.UserId == userId,
            include: q => q.Include(r => r.User)
                          .Include(r => r.Session),
            orderBy: q => q.OrderByDescending(r => r.CreatedAt),
            cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<Review>> GetReviewsBySessionAsync(Guid sessionId, CancellationToken cancellationToken = default)
    {
        return await GetListAsync(
            predicate: r => r.SessionId == sessionId,
            include: q => q.Include(r => r.User)
                          .Include(r => r.Session),
            orderBy: q => q.OrderByDescending(r => r.CreatedAt),
            cancellationToken: cancellationToken);
    }

    public async Task<double> GetAverageRatingBySessionAsync(Guid sessionId, CancellationToken cancellationToken = default)
    {
        var reviews = await GetListAsync(
            predicate: r => r.SessionId == sessionId,
            enableTracking: false,
            cancellationToken: cancellationToken);

        return reviews.Any() ? reviews.Average(r => r.Rating) : 0;
    }
} 