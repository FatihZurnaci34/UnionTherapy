namespace UnionTherapy.Application.Models.Review.Request;

public class CreateReviewRequest
{
    public Guid SessionId { get; set; }
    public Guid UserId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public bool IsAnonymous { get; set; }
} 