namespace UnionTherapy.Application.Models.Psychologist.Response;

public class PsychologistGetListResponse
{
    public IEnumerable<PsychologistGetByIdResponse> Psychologists { get; set; } = new List<PsychologistGetByIdResponse>();
    public int TotalCount { get; set; }
} 