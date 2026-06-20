using Quiz.Shared.Enums;

namespace Quiz.Shared.DTO.Question.Request;
public class QuestionCreateRequestDto
{
    public Guid GroupId { get; set; }

    public List<McqQuestion> mcq { get; set; } = new();

    public List<MsqQuestion> msq { get; set; } = new();

    public List<TrueFalseQuestion> tf { get; set; } = new();

    public List<ShortQuestion> sa { get; set; } = new();
}

public abstract class BaseQuestion
{
    public QuestionTypeEnum Type { get; set; }

    public string Text { get; set; } = string.Empty;

    public int Points { get; set; }
}

public class McqQuestion : BaseQuestion
{
    public List<string> Options { get; set; } = new();

    public int CorrectAnswer { get; set; }
}

public class MsqQuestion : BaseQuestion
{
    public List<string> Options { get; set; } = new();

    public List<int> CorrectAnswer { get; set; } = new();
}

public class TrueFalseQuestion : BaseQuestion
{
    public bool CorrectAnswer { get; set; }
}

public class ShortQuestion : BaseQuestion
{
}