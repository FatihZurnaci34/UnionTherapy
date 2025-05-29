using UnionTherapy.Domain.Common;

namespace UnionTherapy.Domain.Entities;

public class Review : BaseEntity<Guid>
{
    public Guid SessionId { get; set; }
    public Guid UserId { get; set; }
    public int Rating { get; set; } // 1-5 range
    public string? Comment { get; set; }
    public bool IsAnonymous { get; set; } = false;
    public bool IsApproved { get; set; } = false;
    public DateTime? ApprovalDate { get; set; }
    public Guid? ApprovedByAdminId { get; set; }
    
    public Review()
    {
        Id = Guid.NewGuid();
    }
    
    // Navigation Properties
    public virtual Session Session { get; set; } = null!;
    public virtual User User { get; set; } = null!;
} 