namespace UnionTherapy.Application.Models.Payment.Request;

public class CreatePaymentRequest
{
    public Guid UserId { get; set; }
    public Guid SessionParticipationId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
} 