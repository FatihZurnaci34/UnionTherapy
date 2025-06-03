namespace UnionTherapy.Application.Models.Session.Response;

public class SessionGetListResponse
{
    public IEnumerable<SessionGetByIdResponse> Sessions { get; set; } = new List<SessionGetByIdResponse>();
    public int TotalCount { get; set; }
} 