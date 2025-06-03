namespace UnionTherapy.Application.Models.Review.Response;

public class ReviewGetByIdResponse
{
    public Guid Id { get; set; }
    public Guid SessionId { get; set; }
    public string SessionName { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public bool IsAnonymous { get; set; }
    public bool IsApproved { get; set; }
    public DateTime? ApprovalDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
} 