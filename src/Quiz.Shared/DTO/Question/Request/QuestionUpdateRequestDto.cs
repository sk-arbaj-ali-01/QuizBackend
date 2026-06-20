using Quiz.Shared.Enums;

namespace Quiz.Shared.DTO.Question.Request;
public class QuestionUpdateRequestDto
{
    public Guid GroupId { get; set; }

    public List<McqQuestionUpdateDto> mcq { get; set; } = new();

    public List<MsqQuestionUpdateDto> msq { get; set; } = new();

    public List<TrueFalseQuestionUpdateDto> tf { get; set; } = new();

    public List<ShortQuestionUpdateDto> sa { get; set; } = new();
}

public abstract class BaseQuestionUpdateDto
{
    public Guid? QuestionId { get; set; }

    public QuestionTypeEnum Type { get; set; }

    public string Text { get; set; } = string.Empty;

    public int Points { get; set; }
}

public class McqQuestionUpdateDto : BaseQuestionUpdateDto
{
    public List<string> Options { get; set; } = new();

    public int CorrectAnswer { get; set; }
}

public class MsqQuestionUpdateDto : BaseQuestionUpdateDto
{
    public List<string> Options { get; set; } = new();

    public List<int> CorrectAnswer { get; set; } = new();
}

public class TrueFalseQuestionUpdateDto : BaseQuestionUpdateDto
{
    public bool CorrectAnswer { get; set; }
}

public class ShortQuestionUpdateDto : BaseQuestionUpdateDto
{
}
