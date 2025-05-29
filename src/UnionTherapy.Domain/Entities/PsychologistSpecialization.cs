using UnionTherapy.Domain.Common;
using UnionTherapy.Domain.Enums;

namespace UnionTherapy.Domain.Entities;

public class PsychologistSpecialization : BaseEntity<Guid>
{
    public Guid PsychologistId { get; set; }
    public SpecializationArea SpecializationArea { get; set; }
    
    public PsychologistSpecialization()
    {
        Id = Guid.NewGuid();
    }
    
    // Navigation Properties
    public virtual Psychologist Psychologist { get; set; } = null!;
} 