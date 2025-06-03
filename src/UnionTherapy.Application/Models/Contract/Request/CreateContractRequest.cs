using UnionTherapy.Domain.Enums;

namespace UnionTherapy.Application.Models.Contract.Request;

public class CreateContractRequest
{
    public Guid? PsychologistId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public ContractType Type { get; set; }
    public DateTime EffectiveDate { get; set; } = DateTime.UtcNow;
    public DateTime? ExpiryDate { get; set; }
} 