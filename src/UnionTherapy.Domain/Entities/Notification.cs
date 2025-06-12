 using UnionTherapy.Domain.Common;
using UnionTherapy.Domain.Enums;

namespace UnionTherapy.Domain.Entities;

public class Notification : BaseEntity<Guid>
{
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
    public bool IsRead { get; set; } = false;
    public DateTime? ReadDate { get; set; }
    public bool IsSent { get; set; } = false;
    public DateTime? SentDate { get; set; }
    public string? ErrorMessage { get; set; }
    public int RetryCount { get; set; } = 0;
    
    public Notification()
    {
        Id = Guid.NewGuid();
    }
    
    // Navigation Properties
    public virtual User User { get; set; } = null!;
} 