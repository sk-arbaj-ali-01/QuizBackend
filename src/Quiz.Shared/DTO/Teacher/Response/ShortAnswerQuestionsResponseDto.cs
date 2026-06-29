namespace Quiz.Shared.DTO.Teacher.Response;
public class ShortAnswerQuestionsResponseDto
{
    public Guid QuestionId { get; set; }

    public string QuestionText { get; set; } = string.Empty;

    public int Points { get; set; }

    public string AnswerText { get; set; } = string.Empty;
}
