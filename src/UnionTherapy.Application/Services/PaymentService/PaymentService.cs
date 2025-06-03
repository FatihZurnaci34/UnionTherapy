using AutoMapper;
using UnionTherapy.Application.Models.Payment.Request;
using UnionTherapy.Application.Models.Payment.Response;
using UnionTherapy.Application.Repository;
using UnionTherapy.Domain.Entities;
using UnionTherapy.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace UnionTherapy.Application.Services.PaymentService;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IMapper _mapper;

    public PaymentService(IPaymentRepository paymentRepository, IMapper mapper)
    {
        _paymentRepository = paymentRepository;
        _mapper = mapper;
    }

    public async Task<PaymentGetByIdResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var payment = await _paymentRepository.GetAsync(
            predicate: p => p.Id == id,
            include: q => q.Include(p => p.User),
            cancellationToken: cancellationToken);

        return payment != null ? _mapper.Map<PaymentGetByIdResponse>(payment) : null;
    }

    public async Task<PaymentGetListResponse> GetPaymentsByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var payments = await _paymentRepository.GetListAsync(
            predicate: p => p.UserId == userId,
            include: q => q.Include(p => p.User),
            orderBy: q => q.OrderByDescending(p => p.CreatedAt),
            cancellationToken: cancellationToken);

        var paymentResponses = _mapper.Map<IEnumerable<PaymentGetByIdResponse>>(payments);
        return new PaymentGetListResponse
        {
            Payments = paymentResponses,
            TotalCount = paymentResponses.Count()
        };
    }

    public async Task<PaymentGetListResponse> GetPaymentsBySessionAsync(Guid sessionId, CancellationToken cancellationToken = default)
    {
        var payments = await _paymentRepository.GetListAsync(
            predicate: p => p.SessionParticipation.SessionId == sessionId,
            include: q => q.Include(p => p.User)
                          .Include(p => p.SessionParticipation),
            orderBy: q => q.OrderByDescending(p => p.CreatedAt),
            cancellationToken: cancellationToken);

        var paymentResponses = _mapper.Map<IEnumerable<PaymentGetByIdResponse>>(payments);
        return new PaymentGetListResponse
        {
            Payments = paymentResponses,
            TotalCount = paymentResponses.Count()
        };
    }

    public async Task<PaymentGetListResponse> GetSuccessfulPaymentsAsync(CancellationToken cancellationToken = default)
    {
        var payments = await _paymentRepository.GetListAsync(
            predicate: p => p.Status == PaymentStatus.Completed,
            include: q => q.Include(p => p.User),
            orderBy: q => q.OrderByDescending(p => p.CreatedAt),
            cancellationToken: cancellationToken);

        var paymentResponses = _mapper.Map<IEnumerable<PaymentGetByIdResponse>>(payments);
        return new PaymentGetListResponse
        {
            Payments = paymentResponses,
            TotalCount = paymentResponses.Count()
        };
    }

    public async Task<PaymentGetListResponse> GetPendingPaymentsAsync(CancellationToken cancellationToken = default)
    {
        var payments = await _paymentRepository.GetListAsync(
            predicate: p => p.Status == PaymentStatus.Pending,
            include: q => q.Include(p => p.User),
            orderBy: q => q.OrderByDescending(p => p.CreatedAt),
            cancellationToken: cancellationToken);

        var paymentResponses = _mapper.Map<IEnumerable<PaymentGetByIdResponse>>(payments);
        return new PaymentGetListResponse
        {
            Payments = paymentResponses,
            TotalCount = paymentResponses.Count()
        };
    }

    public async Task<PaymentGetListResponse> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        var payments = await _paymentRepository.GetListAsync(
            predicate: p => p.CreatedAt >= startDate && p.CreatedAt <= endDate,
            include: q => q.Include(p => p.User),
            orderBy: q => q.OrderByDescending(p => p.CreatedAt),
            cancellationToken: cancellationToken);

        var paymentResponses = _mapper.Map<IEnumerable<PaymentGetByIdResponse>>(payments);
        return new PaymentGetListResponse
        {
            Payments = paymentResponses,
            TotalCount = paymentResponses.Count()
        };
    }

    public async Task<CreatePaymentResponse> CreateAsync(CreatePaymentRequest request, CancellationToken cancellationToken = default)
    {
        var payment = _mapper.Map<Payment>(request);
        payment.CreatedAt = DateTime.UtcNow;
        payment.Status = PaymentStatus.Pending;

        var createdPayment = await _paymentRepository.AddAsync(payment, cancellationToken);
        await _paymentRepository.SaveChangesAsync(cancellationToken);

        var paymentResponse = _mapper.Map<PaymentGetByIdResponse>(createdPayment);
        return new CreatePaymentResponse { Payment = paymentResponse };
    }

    public async Task<PaymentGetByIdResponse?> UpdateAsync(Guid id, CreatePaymentRequest request, CancellationToken cancellationToken = default)
    {
        var existingPayment = await _paymentRepository.GetByIdAsync(id, cancellationToken);
        if (existingPayment == null)
            return null;

        _mapper.Map(request, existingPayment);
        existingPayment.UpdatedAt = DateTime.UtcNow;

        var updatedPayment = await _paymentRepository.UpdateAsync(existingPayment, cancellationToken);
        await _paymentRepository.SaveChangesAsync(cancellationToken);

        return _mapper.Map<PaymentGetByIdResponse>(updatedPayment);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _paymentRepository.DeleteAsync(id, cancellationToken);
        await _paymentRepository.SaveChangesAsync(cancellationToken);
        return true;
    }
} 