namespace Quiz.Shared.DTO.Group.Response;
public class GroupResponseDto
{
    public Guid GroupId { get; set; }

    public string GroupName { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public bool IsArchived { get; set; }

    public int ExamDuration { get; set; }

    public DateTime CreatedAt { get; set; }

    public int ActiveForDays { get; set; }
}
