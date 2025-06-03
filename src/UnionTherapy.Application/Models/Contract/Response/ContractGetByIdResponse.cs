using UnionTherapy.Domain.Enums;

namespace UnionTherapy.Application.Models.Contract.Response;

public class ContractGetByIdResponse
{
    public Guid Id { get; set; }
    public Guid? PsychologistId { get; set; }
    public string? PsychologistName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public ContractType Type { get; set; }
    public ContractStatus Status { get; set; }
    public string Version { get; set; } = string.Empty;
    public DateTime EffectiveDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
} 