using AutoMapper;
using UnionTherapy.Application.Models.Session.Request;
using UnionTherapy.Application.Models.Session.Response;
using UnionTherapy.Application.Repository;
using UnionTherapy.Domain.Entities;
using UnionTherapy.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace UnionTherapy.Application.Services.SessionService;

public class SessionService : ISessionService
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IMapper _mapper;

    public SessionService(ISessionRepository sessionRepository, IMapper mapper)
    {
        _sessionRepository = sessionRepository;
        _mapper = mapper;
    }

    public async Task<SessionGetByIdResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var session = await _sessionRepository.GetAsync(
            predicate: s => s.Id == id,
            include: q => q.Include(s => s.Psychologist)
                          .ThenInclude(p => p.User)
                          .Include(s => s.Participants),
            cancellationToken: cancellationToken);

        return session != null ? _mapper.Map<SessionGetByIdResponse>(session) : null;
    }

    public async Task<SessionGetListResponse> GetSessionsByPsychologistAsync(Guid psychologistId, CancellationToken cancellationToken = default)
    {
        var sessions = await _sessionRepository.GetListAsync(
            predicate: s => s.PsychologistId == psychologistId,
            include: q => q.Include(s => s.Psychologist)
                          .ThenInclude(p => p.User)
                          .Include(s => s.Participants),
            orderBy: q => q.OrderBy(s => s.StartDate),
            cancellationToken: cancellationToken);

        var sessionResponses = _mapper.Map<IEnumerable<SessionGetByIdResponse>>(sessions);
        return new SessionGetListResponse 
        { 
            Sessions = sessionResponses,
            TotalCount = sessionResponses.Count()
        };
    }

    public async Task<SessionGetListResponse> GetSessionsByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var sessions = await _sessionRepository.GetListAsync(
            predicate: s => s.Participants.Any(sp => sp.UserId == userId),
            include: q => q.Include(s => s.Participants)
                          .Include(s => s.Psychologist)
                          .ThenInclude(p => p.User),
            orderBy: q => q.OrderBy(s => s.StartDate),
            cancellationToken: cancellationToken);

        var sessionResponses = _mapper.Map<IEnumerable<SessionGetByIdResponse>>(sessions);
        return new SessionGetListResponse 
        { 
            Sessions = sessionResponses,
            TotalCount = sessionResponses.Count()
        };
    }

    public async Task<SessionGetListResponse> GetUpcomingSessionsAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var sessions = await _sessionRepository.GetListAsync(
            predicate: s => s.StartDate > now,
            include: q => q.Include(s => s.Psychologist)
                          .ThenInclude(p => p.User)
                          .Include(s => s.Participants),
            orderBy: q => q.OrderBy(s => s.StartDate),
            cancellationToken: cancellationToken);

        var sessionResponses = _mapper.Map<IEnumerable<SessionGetByIdResponse>>(sessions);
        return new SessionGetListResponse 
        { 
            Sessions = sessionResponses,
            TotalCount = sessionResponses.Count()
        };
    }

    public async Task<SessionGetListResponse> GetSessionsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        var sessions = await _sessionRepository.GetListAsync(
            predicate: s => s.StartDate >= startDate && s.StartDate <= endDate,
            include: q => q.Include(s => s.Psychologist)
                          .ThenInclude(p => p.User)
                          .Include(s => s.Participants),
            orderBy: q => q.OrderBy(s => s.StartDate),
            cancellationToken: cancellationToken);

        var sessionResponses = _mapper.Map<IEnumerable<SessionGetByIdResponse>>(sessions);
        return new SessionGetListResponse 
        { 
            Sessions = sessionResponses,
            TotalCount = sessionResponses.Count()
        };
    }

    public async Task<SessionGetByIdResponse?> GetSessionWithParticipantsAsync(Guid sessionId, CancellationToken cancellationToken = default)
    {
        var session = await _sessionRepository.GetAsync(
            predicate: s => s.Id == sessionId,
            include: q => q.Include(s => s.Psychologist)
                          .ThenInclude(p => p.User)
                          .Include(s => s.Participants)
                          .ThenInclude(sp => sp.User),
            cancellationToken: cancellationToken);

        return session != null ? _mapper.Map<SessionGetByIdResponse>(session) : null;
    }

    public async Task<SessionGetListResponse> GetActiveSessionsAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var sessions = await _sessionRepository.GetListAsync(
            predicate: s => s.StartDate <= now && s.EndDate >= now,
            include: q => q.Include(s => s.Psychologist)
                          .ThenInclude(p => p.User)
                          .Include(s => s.Participants),
            cancellationToken: cancellationToken);

        var sessionResponses = _mapper.Map<IEnumerable<SessionGetByIdResponse>>(sessions);
        return new SessionGetListResponse 
        { 
            Sessions = sessionResponses,
            TotalCount = sessionResponses.Count()
        };
    }

    public async Task<CreateSessionResponse> CreateAsync(CreateSessionRequest request, CancellationToken cancellationToken = default)
    {
        var session = _mapper.Map<Session>(request);
        session.CreatedAt = DateTime.UtcNow;
        session.Status = SessionStatus.Planned;

        var createdSession = await _sessionRepository.AddAsync(session, cancellationToken);
        await _sessionRepository.SaveChangesAsync(cancellationToken);

        var sessionResponse = _mapper.Map<SessionGetByIdResponse>(createdSession);
        return new CreateSessionResponse { Session = sessionResponse };
    }

    public async Task<SessionGetByIdResponse?> UpdateAsync(Guid id, UpdateSessionRequest request, CancellationToken cancellationToken = default)
    {
        var existingSession = await _sessionRepository.GetByIdAsync(id, cancellationToken);
        if (existingSession == null)
            return null;

        _mapper.Map(request, existingSession);
        existingSession.UpdatedAt = DateTime.UtcNow;

        var updatedSession = await _sessionRepository.UpdateAsync(existingSession, cancellationToken);
        await _sessionRepository.SaveChangesAsync(cancellationToken);

        return _mapper.Map<SessionGetByIdResponse>(updatedSession);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _sessionRepository.DeleteAsync(id, cancellationToken);
        await _sessionRepository.SaveChangesAsync(cancellationToken);
        return true;
    }
} 