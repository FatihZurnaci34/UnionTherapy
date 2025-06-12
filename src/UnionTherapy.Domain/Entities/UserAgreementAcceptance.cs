using UnionTherapy.Domain.Common;

namespace UnionTherapy.Domain.Entities;

public class UserAgreementAcceptance : BaseEntity<Guid>
{
    public Guid UserId { get; set; }
    public Guid UserAgreementId { get; set; }
    public bool IsAccepted { get; set; } = false;
    public DateTime AcceptanceDate { get; set; } = DateTime.UtcNow;
    public string IpAddress { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public string AgreementVersion { get; set; } = string.Empty; // Kabul edilen sürüm
    
    public UserAgreementAcceptance()
    {
        Id = Guid.NewGuid();
    }
    
    // Navigation Properties
    public virtual User User { get; set; } = null!;
    public virtual UserAgreement UserAgreement { get; set; } = null!;
} 