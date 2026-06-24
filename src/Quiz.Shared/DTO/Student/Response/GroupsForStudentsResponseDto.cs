namespace Quiz.Shared.DTO.Student.Response;
public class GroupsForStudentsResponseDto
{
    public Guid GroupId { get; set; }

    public string GroupName { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int ExamDuration { get; set; }

    public int TotalPoints { get; set; }
}
