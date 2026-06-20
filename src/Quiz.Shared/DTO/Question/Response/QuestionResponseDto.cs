namespace Quiz.Shared.DTO.Question.Response;
public class QuestionResponseDto
{
    public Guid GroupId { get; set; }

    public int ExamDuration { get; set; }

    public List<McqQuestionResponseDto> mcq { get; set; } = new();

    public List<MsqQuestionResponseDto> msq { get; set; } = new();

    public List<TrueFalseQuestionResponseDto> tf { get; set; } = new();

    public List<ShortQuestionResponseDto> sa { get; set; } = new();
}

public abstract class BaseQuestionResponseDto
{
    public Guid QuestionId { get; set; }

    public string Type { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;

    public int Points { get; set; }
}

public class McqQuestionResponseDto : BaseQuestionResponseDto
{
    public List<QuestionOptionResponseDto> Options { get; set; } = new();
}

public class MsqQuestionResponseDto : BaseQuestionResponseDto
{
    public List<QuestionOptionResponseDto> Options { get; set; } = new();
}

public class TrueFalseQuestionResponseDto : BaseQuestionResponseDto
{
    public bool CorrectAnswer { get; set; }
}

public class ShortQuestionResponseDto : BaseQuestionResponseDto
{
}

public class QuestionOptionResponseDto
{
    public Guid OptionId { get; set; }

    public string OptionText { get; set; } = string.Empty;

    public bool IsCorrect { get; set; }
}
