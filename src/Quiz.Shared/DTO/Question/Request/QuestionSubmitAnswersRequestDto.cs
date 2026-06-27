using Quiz.Shared.Enums;

namespace Quiz.Shared.DTO.Question.Request;
public class QuestionSubmitAnswersRequestDto
{
    public Guid GroupId { get; set; }

    public List<QuestionMcqAnswerRequestDto> McqAnswers { get; set; } = new();
    public List<QuestionMsqAnswerRequestDto> MsqAnswers { get; set; } = new();
    public List<QuestionTrueFalseAnswerRequestDto> TrueFalseAnswers { get; set; } = new();
    public List<QuestionShortAnswerAnswerRequestDto> ShortAnswers { get; set; } = new();
}

public class QuestionMcqAnswerRequestDto
{
    public Guid QuestionId { get; set; }

    public QuestionTypeEnum QuestionType { get; set; }

    public Guid? OptionId { get; set; }
}
public class QuestionMsqAnswerRequestDto
{
    public Guid QuestionId { get; set; }

    public QuestionTypeEnum QuestionType { get; set; }

    public List<Guid> OptionIds { get; set; } = new();
}
public class QuestionTrueFalseAnswerRequestDto
{
    public Guid QuestionId { get; set; }

    public QuestionTypeEnum QuestionType { get; set; }

    public bool Answer { get; set; }
}
public class QuestionShortAnswerAnswerRequestDto
{
    public Guid QuestionId { get; set; }

    public QuestionTypeEnum QuestionType { get; set; }

    public string Answer { get; set; } = string.Empty;
}
