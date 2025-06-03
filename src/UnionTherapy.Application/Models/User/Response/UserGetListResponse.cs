namespace UnionTherapy.Application.Models.User.Response;

public class UserGetListResponse
{
    public IEnumerable<UserGetByIdResponse> Users { get; set; } = new List<UserGetByIdResponse>();
    public int TotalCount { get; set; }
} 