using UnionTherapy.Domain.Enums;

namespace UnionTherapy.Application.Models.Payment.Response;

public class PaymentGetByIdResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public Guid SessionParticipationId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public PaymentStatus Status { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string? PaymentServiceId { get; set; }
    public string? PaymentServiceReference { get; set; }
    public DateTime? PaymentDate { get; set; }
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
} 