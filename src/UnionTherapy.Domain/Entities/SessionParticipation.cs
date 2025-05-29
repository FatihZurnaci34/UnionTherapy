using UnionTherapy.Domain.Common;

namespace UnionTherapy.Domain.Entities;

public class SessionParticipation : BaseEntity<Guid>
{
    public Guid SessionId { get; set; }
    public Guid UserId { get; set; }
    public bool IsAnonymous { get; set; } = false;
    public string? AnonymousName { get; set; }
    public DateTime ParticipationDate { get; set; } = DateTime.UtcNow;
    public bool HasAttended { get; set; } = false;
    public DateTime? AttendanceStart { get; set; }
    public DateTime? AttendanceEnd { get; set; }
    public string? Notes { get; set; }
    
    public SessionParticipation()
    {
        Id = Guid.NewGuid();
    }
    
    // Navigation Properties
    public virtual Session Session { get; set; } = null!;
    public virtual User User { get; set; } = null!;
    public virtual Payment? Payment { get; set; }
} 