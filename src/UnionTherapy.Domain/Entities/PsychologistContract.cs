using UnionTherapy.Domain.Common;
using UnionTherapy.Domain.Enums;

namespace UnionTherapy.Domain.Entities;

public class PsychologistContract : BaseEntity<Guid>
{
    public Guid PsychologistId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public PsychologistContractType Type { get; set; } // ServiceAgreement, CommissionAgreement vs.
    public PsychologistContractStatus Status { get; set; } = PsychologistContractStatus.Draft;
    public string Version { get; set; } = "1.0";
    public DateTime EffectiveDate { get; set; } = DateTime.UtcNow;
    public DateTime? ExpiryDate { get; set; }
    public DateTime? SignedDate { get; set; }
    public decimal? CommissionRate { get; set; } // Komisyon oranı
    public decimal? MonthlyFee { get; set; } // Aylık ücret
    public string? SpecialTerms { get; set; } // Özel şartlar
    
    public PsychologistContract()
    {
        Id = Guid.NewGuid();
    }
    
    // Navigation Properties
    public virtual Psychologist Psychologist { get; set; } = null!;
} 