using UnionTherapy.Domain.Common;

namespace UnionTherapy.Domain.Entities;

public class Contract : BaseEntity<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // GDPR, User Agreement, Psychologist Agreement
    public string Version { get; set; } = "1.0";
    public bool IsActive { get; set; } = true;
    public DateTime EffectiveDate { get; set; } = DateTime.UtcNow;
    
    public Contract()
    {
        Id = Guid.NewGuid();
    }
    
    // Navigation Properties
    public virtual ICollection<UserContract> UserContracts { get; set; } = new List<UserContract>();
} 