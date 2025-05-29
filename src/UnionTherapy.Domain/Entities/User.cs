using UnionTherapy.Domain.Common;
using UnionTherapy.Domain.Enums;

namespace UnionTherapy.Domain.Entities;

public class User : BaseEntity<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public UserRole Role { get; set; } = UserRole.User;
    public bool IsEmailVerified { get; set; } = false;
    public bool IsPhoneVerified { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public DateTime? LastLoginDate { get; set; }
    public string? ProfileImagePath { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; } = "Turkey";
    
    public User()
    {
        Id = Guid.NewGuid();
    }
    
    // Navigation Properties
    public virtual Psychologist? Psychologist { get; set; }
    public virtual ICollection<SessionParticipation> SessionParticipations { get; set; } = new List<SessionParticipation>();
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    public virtual ICollection<PsychologistReview> PsychologistReviews { get; set; } = new List<PsychologistReview>();
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public virtual ICollection<UserContract> UserContracts { get; set; } = new List<UserContract>();
} 