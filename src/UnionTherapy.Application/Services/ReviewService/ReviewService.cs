using AutoMapper;
using UnionTherapy.Application.Models.Review.Request;
using UnionTherapy.Application.Models.Review.Response;
using UnionTherapy.Application.Repository;
using UnionTherapy.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace UnionTherapy.Application.Services.ReviewService;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IMapper _mapper;

    public ReviewService(IReviewRepository reviewRepository, IMapper mapper)
    {
        _reviewRepository = reviewRepository;
        _mapper = mapper;
    }

    public async Task<ReviewGetByIdResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var review = await _reviewRepository.GetAsync(
            predicate: r => r.Id == id,
            include: q => q.Include(r => r.User)
                          .Include(r => r.Session),
            cancellationToken: cancellationToken);

        return review != null ? _mapper.Map<ReviewGetByIdResponse>(review) : null;
    }

    public async Task<IEnumerable<ReviewGetByIdResponse>> GetReviewsBySessionAsync(Guid sessionId, CancellationToken cancellationToken = default)
    {
        var reviews = await _reviewRepository.GetListAsync(
            predicate: r => r.SessionId == sessionId,
            include: q => q.Include(r => r.User)
                          .Include(r => r.Session),
            orderBy: q => q.OrderByDescending(r => r.CreatedAt),
            cancellationToken: cancellationToken);

        return _mapper.Map<IEnumerable<ReviewGetByIdResponse>>(reviews);
    }

    public async Task<IEnumerable<ReviewGetByIdResponse>> GetReviewsByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var reviews = await _reviewRepository.GetListAsync(
            predicate: r => r.UserId == userId,
            include: q => q.Include(r => r.User)
                          .Include(r => r.Session),
            orderBy: q => q.OrderByDescending(r => r.CreatedAt),
            cancellationToken: cancellationToken);

        return _mapper.Map<IEnumerable<ReviewGetByIdResponse>>(reviews);
    }

    public async Task<IEnumerable<ReviewGetByIdResponse>> GetApprovedReviewsAsync(CancellationToken cancellationToken = default)
    {
        var reviews = await _reviewRepository.GetListAsync(
            predicate: r => r.IsApproved,
            include: q => q.Include(r => r.User)
                          .Include(r => r.Session),
            orderBy: q => q.OrderByDescending(r => r.CreatedAt),
            cancellationToken: cancellationToken);

        return _mapper.Map<IEnumerable<ReviewGetByIdResponse>>(reviews);
    }

    public async Task<IEnumerable<ReviewGetByIdResponse>> GetPendingReviewsAsync(CancellationToken cancellationToken = default)
    {
        var reviews = await _reviewRepository.GetListAsync(
            predicate: r => !r.IsApproved,
            include: q => q.Include(r => r.User)
                          .Include(r => r.Session),
            orderBy: q => q.OrderBy(r => r.CreatedAt),
            cancellationToken: cancellationToken);

        return _mapper.Map<IEnumerable<ReviewGetByIdResponse>>(reviews);
    }

    public async Task<double> GetAverageRatingBySessionAsync(Guid sessionId, CancellationToken cancellationToken = default)
    {
        var reviews = await _reviewRepository.GetListAsync(
            predicate: r => r.SessionId == sessionId && r.IsApproved,
            enableTracking: false,
            cancellationToken: cancellationToken);

        return reviews.Any() ? reviews.Average(r => r.Rating) : 0;
    }

    public async Task<ReviewGetByIdResponse> CreateAsync(CreateReviewRequest request, CancellationToken cancellationToken = default)
    {
        var review = _mapper.Map<Review>(request);
        review.CreatedAt = DateTime.UtcNow;
        review.IsApproved = false;

        var createdReview = await _reviewRepository.AddAsync(review, cancellationToken);
        await _reviewRepository.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ReviewGetByIdResponse>(createdReview);
    }

    public async Task<ReviewGetByIdResponse?> UpdateAsync(Guid id, CreateReviewRequest request, CancellationToken cancellationToken = default)
    {
        var existingReview = await _reviewRepository.GetByIdAsync(id, cancellationToken);
        if (existingReview == null)
            return null;

        _mapper.Map(request, existingReview);
        existingReview.UpdatedAt = DateTime.UtcNow;

        var updatedReview = await _reviewRepository.UpdateAsync(existingReview, cancellationToken);
        await _reviewRepository.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ReviewGetByIdResponse>(updatedReview);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _reviewRepository.DeleteAsync(id, cancellationToken);
        await _reviewRepository.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> ApproveAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var review = await _reviewRepository.GetByIdAsync(id, cancellationToken);
        if (review == null)
            return false;

        review.IsApproved = true;
        review.ApprovalDate = DateTime.UtcNow;
        review.UpdatedAt = DateTime.UtcNow;

        await _reviewRepository.UpdateAsync(review, cancellationToken);
        await _reviewRepository.SaveChangesAsync(cancellationToken);
        return true;
    }
} 