namespace UnionTherapy.Application.Models.Payment.Response;

public class PaymentGetListResponse
{
    public IEnumerable<PaymentGetByIdResponse> Payments { get; set; } = new List<PaymentGetByIdResponse>();
    public int TotalCount { get; set; }
} 