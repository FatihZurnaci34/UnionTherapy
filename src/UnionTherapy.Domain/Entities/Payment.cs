using UnionTherapy.Domain.Common;
using UnionTherapy.Domain.Enums;

namespace UnionTherapy.Domain.Entities;

public class Payment : BaseEntity<Guid>
{
    public Guid UserId { get; set; }
    public Guid SessionParticipationId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "TRY";
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    public string PaymentMethod { get; set; } = string.Empty; // Credit Card, Debit Card etc.
    public string? PaymentServiceId { get; set; } // Stripe, iyzico etc. service ID
    public string? PaymentServiceReference { get; set; }
    public DateTime? PaymentDate { get; set; }
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
    public string? RefundReason { get; set; }
    public DateTime? RefundDate { get; set; }
    public decimal? RefundAmount { get; set; }
    
    public Payment()
    {
        Id = Guid.NewGuid();
    }
    
    // Navigation Properties
    public virtual User User { get; set; } = null!;
    public virtual SessionParticipation SessionParticipation { get; set; } = null!;
} 