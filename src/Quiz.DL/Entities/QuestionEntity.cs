using Quiz.Shared.Enums;

namespace Quiz.DL.Entities;
public class QuestionEntity
{
    public Guid GroupId { get; set; }

    public List<McqQuestionEntity> mcq { get; set; } = new();

    public List<MsqQuestionEntity> msq { get; set; } = new();

    public List<TrueFalseQuestionEntity> tf { get; set; } = new();

    public List<ShortQuestionEntity> sa { get; set; } = new();
}

public abstract class BaseQuestionEntity
{
    public Guid QuestionId { get; set; }

    public string Type { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;

    public int Points { get; set; }
}

public class McqQuestionEntity : BaseQuestionEntity
{
    public List<string> Options { get; set; } = new();

    public int CorrectAnswer { get; set; }
}

public class MsqQuestionEntity : BaseQuestionEntity
{
    public List<string> Options { get; set; } = new();

    public List<int> CorrectAnswer { get; set; } = new();
}

public class TrueFalseQuestionEntity : BaseQuestionEntity
{
    public bool CorrectAnswer { get; set; }
}

public class ShortQuestionEntity : BaseQuestionEntity
{
}
