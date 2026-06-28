namespace Quiz.Shared.DTO.Student.Response;
public class AttemptedQuizzesResponseDto
{
    public Guid GroupId { get; set; }

    public string GroupName { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int TotalPoints { get; set; }

    public bool UnderReview { get; set; }

    public int? MarksObtained { get; set; }
}