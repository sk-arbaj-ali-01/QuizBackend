using Quiz.Shared.Enums;

namespace Quiz.Shared.DTO.Question.Request;
public class QuestionSubmitAnswersRequestDto
{
    public Guid GroupId { get; set; }

    public List<QuestionAnswerRequestDto> Questions { get; set; } = new();
}

public class QuestionAnswerRequestDto
{
    public Guid QuestionId { get; set; }

    public QuestionTypeEnum QuestionType { get; set; }

    public Guid? OptionId { get; set; }
}
