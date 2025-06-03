using UnionTherapy.Domain.Enums;

namespace UnionTherapy.Application.Models.Session.Response;

public class SessionGetByIdResponse
{
    public Guid Id { get; set; }
    public Guid PsychologistId { get; set; }
    public string PsychologistName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int MaxParticipants { get; set; }
    public int CurrentParticipants { get; set; }
    public decimal Price { get; set; }
    public SessionType Type { get; set; }
    public SessionStatus Status { get; set; }
    public string? MeetingLink { get; set; }
    public string? MeetingPassword { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
} 