using UnionTherapy.Domain.Common;

namespace UnionTherapy.Domain.Entities;

public class PsychologistReview : BaseEntity<Guid>
{
    public Guid PsychologistId { get; set; }
    public Guid UserId { get; set; }
    public Guid SessionId { get; set; }
    public int Rating { get; set; } // 1-5 range
    public string? Comment { get; set; }
    public bool IsAnonymous { get; set; } = false;
    public bool IsApproved { get; set; } = false;
    public DateTime? ApprovalDate { get; set; }
    public Guid? ApprovedByAdminId { get; set; }
    public string? ApprovalNote { get; set; }
    
    public PsychologistReview()
    {
        Id = Guid.NewGuid();
    }
    
    // Navigation Properties
    public virtual Psychologist Psychologist { get; set; } = null!;
    public virtual User User { get; set; } = null!;
    public virtual Session Session { get; set; } = null!;
} 