namespace Quiz.DL.Entities;
public class GroupEntity : BaseEntity
{
    public Guid GroupId { get; set; }

    public string GroupName { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public int ActiveForDays { get; set; }

    public int ExamDuration { get; set; }

    public bool IsArchived { get; set; }
}
