using UnionTherapy.Domain.Common;
using UnionTherapy.Domain.Enums;

namespace UnionTherapy.Domain.Entities;

public class PsychologistDocument : BaseEntity<Guid>
{
    public Guid PsychologistId { get; set; }
    public string DocumentName { get; set; } = string.Empty;
    public string DocumentType { get; set; } = string.Empty; // Diploma, Certificate, Legal License etc.
    public string FilePath { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string MimeType { get; set; } = string.Empty;
    public DocumentStatus Status { get; set; } = DocumentStatus.Pending;
    public DateTime? ApprovalDate { get; set; }
    public Guid? ApprovedByAdminId { get; set; }
    public string? ApprovalNote { get; set; }
    
    public PsychologistDocument()
    {
        Id = Guid.NewGuid();
    }
    
    // Navigation Properties
    public virtual Psychologist Psychologist { get; set; } = null!;
} 