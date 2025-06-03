using UnionTherapy.Application.Models.Review.Request;
using UnionTherapy.Application.Models.Review.Response;

namespace UnionTherapy.Application.Services.ReviewService;

public interface IReviewService
{
    Task<ReviewGetByIdResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ReviewGetByIdResponse>> GetReviewsBySessionAsync(Guid sessionId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ReviewGetByIdResponse>> GetReviewsByUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ReviewGetByIdResponse>> GetApprovedReviewsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ReviewGetByIdResponse>> GetPendingReviewsAsync(CancellationToken cancellationToken = default);
    Task<double> GetAverageRatingBySessionAsync(Guid sessionId, CancellationToken cancellationToken = default);
    
    Task<ReviewGetByIdResponse> CreateAsync(CreateReviewRequest request, CancellationToken cancellationToken = default);
    Task<ReviewGetByIdResponse?> UpdateAsync(Guid id, CreateReviewRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ApproveAsync(Guid id, CancellationToken cancellationToken = default);
} 