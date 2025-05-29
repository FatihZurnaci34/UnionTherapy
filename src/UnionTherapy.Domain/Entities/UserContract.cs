using UnionTherapy.Domain.Common;

namespace UnionTherapy.Domain.Entities;

public class UserContract : BaseEntity<Guid>
{
    public Guid UserId { get; set; }
    public Guid ContractId { get; set; }
    public bool IsAccepted { get; set; } = false;
    public DateTime AcceptanceDate { get; set; } = DateTime.UtcNow;
    public string IpAddress { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    
    public UserContract()
    {
        Id = Guid.NewGuid();
    }
    
    // Navigation Properties
    public virtual User User { get; set; } = null!;
    public virtual Contract Contract { get; set; } = null!;
} 