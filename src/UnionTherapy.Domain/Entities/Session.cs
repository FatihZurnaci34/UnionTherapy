using UnionTherapy.Domain.Common;
using UnionTherapy.Domain.Enums;

namespace UnionTherapy.Domain.Entities;

public class Session : BaseEntity<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Objective { get; set; } = string.Empty;
    public SpecializationArea SpecializationArea { get; set; }
    public Guid PsychologistId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int MaxParticipants { get; set; }
    public decimal Price { get; set; }
    public bool AnonymousParticipationAvailable { get; set; } = true;
    public SessionStatus Status { get; set; } = SessionStatus.Planned;
    public string? ZoomMeetingId { get; set; }
    public string? ZoomJoinUrl { get; set; }
    public string? ZoomPassword { get; set; }
    public DateTime? ZoomStartTime { get; set; }
    
    public Session()
    {
        Id = Guid.NewGuid();
    }
    
    // Navigation Properties
    public virtual Psychologist Psychologist { get; set; } = null!;
    public virtual ICollection<SessionParticipation> Participants { get; set; } = new List<SessionParticipation>();
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    public virtual ICollection<PsychologistReview> PsychologistReviews { get; set; } = new List<PsychologistReview>();
} 