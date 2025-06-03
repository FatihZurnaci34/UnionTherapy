using UnionTherapy.Domain.Entities;

namespace UnionTherapy.Application.Repository;

public interface IPsychologistDocumentRepository : IBaseRepository<PsychologistDocument>
{
    Task<IEnumerable<PsychologistDocument>> GetDocumentsByPsychologistAsync(Guid psychologistId, CancellationToken cancellationToken = default);
    Task<IEnumerable<PsychologistDocument>> GetDocumentsByTypeAsync(string documentType, CancellationToken cancellationToken = default);
    Task<IEnumerable<PsychologistDocument>> GetVerifiedDocumentsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<PsychologistDocument>> GetPendingDocumentsAsync(CancellationToken cancellationToken = default);
} 