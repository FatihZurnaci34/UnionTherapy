using UnionTherapy.Domain.Common;
using UnionTherapy.Domain.Enums;

namespace UnionTherapy.Domain.Entities;

public class UserAgreement : BaseEntity<Guid>
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public UserAgreementType Type { get; set; } // PrivacyPolicy, TermsOfService, CookiePolicy vs.
    public UserAgreementStatus Status { get; set; } = UserAgreementStatus.Draft;
    public string Version { get; set; } = "1.0";
    public DateTime EffectiveDate { get; set; } = DateTime.UtcNow;
    public DateTime? ExpiryDate { get; set; }
    public bool IsRequired { get; set; } = true; // Zorunlu mu?
    public int DisplayOrder { get; set; } = 0; // Gösterim sırası
    
    public UserAgreement()
    {
        Id = Guid.NewGuid();
    }
    
    // Navigation Properties
    public virtual ICollection<UserAgreementAcceptance> UserAcceptances { get; set; } = new List<UserAgreementAcceptance>();
} 