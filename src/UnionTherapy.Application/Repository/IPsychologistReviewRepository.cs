using UnionTherapy.Domain.Entities;

namespace UnionTherapy.Application.Repository;

public interface IPsychologistReviewRepository : IBaseRepository<PsychologistReview>
{
    Task<IEnumerable<PsychologistReview>> GetReviewsByPsychologistAsync(Guid psychologistId, CancellationToken cancellationToken = default);
    Task<IEnumerable<PsychologistReview>> GetReviewsByUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<double> GetAverageRatingByPsychologistAsync(Guid psychologistId, CancellationToken cancellationToken = default);
    Task<int> GetReviewCountByPsychologistAsync(Guid psychologistId, CancellationToken cancellationToken = default);
} 