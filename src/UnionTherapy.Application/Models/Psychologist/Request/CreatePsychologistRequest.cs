namespace UnionTherapy.Application.Models.Psychologist.Request;

public class CreatePsychologistRequest
{
    public Guid UserId { get; set; }
    public string LicenseNumber { get; set; } = string.Empty;
    public string University { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public int GraduationYear { get; set; }
    public int ExperienceYears { get; set; }
    public string? Biography { get; set; }
    public string? Approach { get; set; }
    public decimal HourlyRate { get; set; }
} 