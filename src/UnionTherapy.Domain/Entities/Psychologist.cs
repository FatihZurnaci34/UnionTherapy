using UnionTherapy.Domain.Common;
using UnionTherapy.Domain.Enums;

namespace UnionTherapy.Domain.Entities;

public class Psychologist : BaseEntity<Guid>
{
    public Guid UserId { get; set; }
    public string LicenseNumber { get; set; } = string.Empty;
    public string University { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public int GraduationYear { get; set; }
    public int ExperienceYears { get; set; }
    public string? Biography { get; set; }
    public string? Approach { get; set; }
    public bool IsApproved { get; set; } = false;
    public DateTime? ApprovalDate { get; set; }
    public Guid? ApprovedByAdminId { get; set; }
    public string? ApprovalNote { get; set; }
    public bool IsActive { get; set; } = true;
    public decimal HourlyRate { get; set; }
    public double AverageRating { get; set; } = 0.0;
    public int TotalReviews { get; set; } = 0;
    public int TotalSessions { get; set; } = 0;
    
    public Psychologist()
    {
        Id = Guid.NewGuid();
    }
    
    // Navigation Properties
    public virtual User User { get; set; } = null!;
    public virtual ICollection<PsychologistSpecialization> Specializations { get; set; } = new List<PsychologistSpecialization>();
    public virtual ICollection<PsychologistDocument> Documents { get; set; } = new List<PsychologistDocument>();
    public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
    public virtual ICollection<PsychologistReview> Reviews { get; set; } = new List<PsychologistReview>();
    public virtual ICollection<PsychologistContract> Contracts { get; set; } = new List<PsychologistContract>();
} 