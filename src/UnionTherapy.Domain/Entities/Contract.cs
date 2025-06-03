using UnionTherapy.Domain.Common;
using UnionTherapy.Domain.Enums;

namespace UnionTherapy.Domain.Entities;

public class Contract : BaseEntity<Guid>
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public ContractType Type { get; set; }
    public ContractStatus Status { get; set; } = ContractStatus.Draft;
    public string Version { get; set; } = "1.0";
    public DateTime EffectiveDate { get; set; } = DateTime.UtcNow;
    public DateTime? ExpiryDate { get; set; }
    public Guid? PsychologistId { get; set; }
    
    public Contract()
    {
        Id = Guid.NewGuid();
    }
    
    // Navigation Properties
    public virtual Psychologist? Psychologist { get; set; }
    public virtual ICollection<UserContract> UserContracts { get; set; } = new List<UserContract>();
} 