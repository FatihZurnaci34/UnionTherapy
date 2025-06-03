using AutoMapper;
using UnionTherapy.Application.Models.Psychologist.Request;
using UnionTherapy.Application.Models.Psychologist.Response;
using UnionTherapy.Application.Repository;
using UnionTherapy.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace UnionTherapy.Application.Services.PsychologistService;

public class PsychologistService : IPsychologistService
{
    private readonly IPsychologistRepository _psychologistRepository;
    private readonly IMapper _mapper;

    public PsychologistService(IPsychologistRepository psychologistRepository, IMapper mapper)
    {
        _psychologistRepository = psychologistRepository;
        _mapper = mapper;
    }

    public async Task<PsychologistGetByIdResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var psychologist = await _psychologistRepository.GetAsync(
            predicate: p => p.Id == id,
            include: q => q.Include(p => p.User),
            cancellationToken: cancellationToken);

        return psychologist != null ? _mapper.Map<PsychologistGetByIdResponse>(psychologist) : null;
    }

    public async Task<PsychologistGetByIdResponse?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var psychologist = await _psychologistRepository.GetAsync(
            predicate: p => p.UserId == userId,
            include: q => q.Include(p => p.User),
            cancellationToken: cancellationToken);

        return psychologist != null ? _mapper.Map<PsychologistGetByIdResponse>(psychologist) : null;
    }

    public async Task<PsychologistGetListResponse> GetApprovedPsychologistsAsync(CancellationToken cancellationToken = default)
    {
        var psychologists = await _psychologistRepository.GetListAsync(
            predicate: p => p.IsApproved,
            include: q => q.Include(p => p.User),
            orderBy: q => q.OrderBy(p => p.User.FirstName).ThenBy(p => p.User.LastName),
            cancellationToken: cancellationToken);

        var psychologistResponses = _mapper.Map<IEnumerable<PsychologistGetByIdResponse>>(psychologists);
        return new PsychologistGetListResponse
        {
            Psychologists = psychologistResponses,
            TotalCount = psychologistResponses.Count()
        };
    }

    public async Task<PsychologistGetListResponse> GetPendingApprovalsAsync(CancellationToken cancellationToken = default)
    {
        var psychologists = await _psychologistRepository.GetListAsync(
            predicate: p => !p.IsApproved,
            include: q => q.Include(p => p.User),
            orderBy: q => q.OrderBy(p => p.CreatedAt),
            cancellationToken: cancellationToken);

        var psychologistResponses = _mapper.Map<IEnumerable<PsychologistGetByIdResponse>>(psychologists);
        return new PsychologistGetListResponse
        {
            Psychologists = psychologistResponses,
            TotalCount = psychologistResponses.Count()
        };
    }

    public async Task<PsychologistGetListResponse> GetActivePsychologistsAsync(CancellationToken cancellationToken = default)
    {
        var psychologists = await _psychologistRepository.GetListAsync(
            predicate: p => p.IsActive && p.IsApproved,
            include: q => q.Include(p => p.User),
            orderBy: q => q.OrderBy(p => p.User.FirstName).ThenBy(p => p.User.LastName),
            cancellationToken: cancellationToken);

        var psychologistResponses = _mapper.Map<IEnumerable<PsychologistGetByIdResponse>>(psychologists);
        return new PsychologistGetListResponse
        {
            Psychologists = psychologistResponses,
            TotalCount = psychologistResponses.Count()
        };
    }

    public async Task<PsychologistGetByIdResponse?> GetPsychologistWithSessionsAsync(Guid psychologistId, CancellationToken cancellationToken = default)
    {
        var psychologist = await _psychologistRepository.GetAsync(
            predicate: p => p.Id == psychologistId,
            include: q => q.Include(p => p.User)
                          .Include(p => p.Sessions),
            cancellationToken: cancellationToken);

        return psychologist != null ? _mapper.Map<PsychologistGetByIdResponse>(psychologist) : null;
    }

    public async Task<CreatePsychologistResponse> CreateAsync(CreatePsychologistRequest request, CancellationToken cancellationToken = default)
    {
        var psychologist = _mapper.Map<Psychologist>(request);
        psychologist.CreatedAt = DateTime.UtcNow;
        psychologist.IsActive = true;
        psychologist.IsApproved = false;

        var createdPsychologist = await _psychologistRepository.AddAsync(psychologist, cancellationToken);
        await _psychologistRepository.SaveChangesAsync(cancellationToken);

        var psychologistResponse = _mapper.Map<PsychologistGetByIdResponse>(createdPsychologist);
        return new CreatePsychologistResponse { Psychologist = psychologistResponse };
    }

    public async Task<PsychologistGetByIdResponse?> UpdateAsync(Guid id, CreatePsychologistRequest request, CancellationToken cancellationToken = default)
    {
        var existingPsychologist = await _psychologistRepository.GetByIdAsync(id, cancellationToken);
        if (existingPsychologist == null)
            return null;

        _mapper.Map(request, existingPsychologist);
        existingPsychologist.UpdatedAt = DateTime.UtcNow;

        var updatedPsychologist = await _psychologistRepository.UpdateAsync(existingPsychologist, cancellationToken);
        await _psychologistRepository.SaveChangesAsync(cancellationToken);

        return _mapper.Map<PsychologistGetByIdResponse>(updatedPsychologist);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _psychologistRepository.DeleteAsync(id, cancellationToken);
        await _psychologistRepository.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> ApproveAsync(Guid id, string approvalNote, CancellationToken cancellationToken = default)
    {
        var psychologist = await _psychologistRepository.GetByIdAsync(id, cancellationToken);
        if (psychologist == null)
            return false;

        psychologist.IsApproved = true;
        psychologist.ApprovalDate = DateTime.UtcNow;
        psychologist.ApprovalNote = approvalNote;
        psychologist.UpdatedAt = DateTime.UtcNow;

        await _psychologistRepository.UpdateAsync(psychologist, cancellationToken);
        await _psychologistRepository.SaveChangesAsync(cancellationToken);
        return true;
    }
} 