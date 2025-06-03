using UnionTherapy.Domain.Entities;
using UnionTherapy.Domain.Enums;

namespace UnionTherapy.Application.Repository;

public interface IPsychologistSpecializationRepository : IBaseRepository<PsychologistSpecialization>
{
    Task<IEnumerable<PsychologistSpecialization>> GetSpecializationsByPsychologistAsync(Guid psychologistId, CancellationToken cancellationToken = default);
    Task<IEnumerable<PsychologistSpecialization>> GetPsychologistsBySpecializationAsync(SpecializationArea specialization, CancellationToken cancellationToken = default);
    Task<bool> HasPsychologistSpecializationAsync(Guid psychologistId, SpecializationArea specialization, CancellationToken cancellationToken = default);
} 