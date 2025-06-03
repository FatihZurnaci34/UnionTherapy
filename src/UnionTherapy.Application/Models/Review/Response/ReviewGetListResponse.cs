namespace UnionTherapy.Application.Models.Review.Response;

public class ReviewGetListResponse
{
    public IEnumerable<ReviewGetByIdResponse> Reviews { get; set; } = new List<ReviewGetByIdResponse>();
    public int TotalCount { get; set; }
} 