namespace Quiz.Shared.DTO.Teacher.Response;
public class ShortAnswerResponseDto
{
    public Guid QuestionId { get; set; }

    public string AnswerText { get; set; } = string.Empty;
}
