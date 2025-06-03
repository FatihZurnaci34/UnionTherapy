using UnionTherapy.Application.Models.Payment.Request;
using UnionTherapy.Application.Models.Payment.Response;

namespace UnionTherapy.Application.Services.PaymentService;

public interface IPaymentService
{
    Task<PaymentGetByIdResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PaymentGetListResponse> GetPaymentsByUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<PaymentGetListResponse> GetPaymentsBySessionAsync(Guid sessionId, CancellationToken cancellationToken = default);
    Task<PaymentGetListResponse> GetSuccessfulPaymentsAsync(CancellationToken cancellationToken = default);
    Task<PaymentGetListResponse> GetPendingPaymentsAsync(CancellationToken cancellationToken = default);
    Task<PaymentGetListResponse> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    
    Task<CreatePaymentResponse> CreateAsync(CreatePaymentRequest request, CancellationToken cancellationToken = default);
    Task<PaymentGetByIdResponse?> UpdateAsync(Guid id, CreatePaymentRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
} 