namespace Quiz.Shared.DTO.Teacher.Response;

public class ExamReviewResponseDto
{
    public Guid GroupId { get; set; }
    public Guid StudentId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public int TotalPoints { get; set; }
}