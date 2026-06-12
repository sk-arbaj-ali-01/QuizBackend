namespace Quiz.Shared.DTO.Group.Request;
public class GroupCreateRequestDto
{
    public string GroupName { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public int ActiveForDays { get; set; }

    public bool IsArchived { get; set; }
}