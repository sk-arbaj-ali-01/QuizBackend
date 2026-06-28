namespace Quiz.DL.Entities;
public class QuestionSubmissionEntity
{
    public Guid GroupId { get; set; }

    public Guid UserId { get; set; }

    public List<QuestionMcqAnswerEntity> McqAnswers { get; set; } = new();
    public List<QuestionMsqAnswerEntity> MsqAnswers { get; set; } = new();
    public List<QuestionTrueFalseAnswerEntity> TrueFalseAnswers { get; set; } = new();
    public List<QuestionShortAnswerAnswerEntity> ShortAnswers { get; set; } = new();
}

public class QuestionMcqAnswerEntity
{
    public Guid QuestionId { get; set; }

    public string QuestionType { get; set; } = string.Empty;

    public Guid? OptionId { get; set; }
}
public class QuestionMsqAnswerEntity
{
    public Guid QuestionId { get; set; }

    public string QuestionType { get; set; } = string.Empty;

    public List<Guid> OptionIds { get; set; } = new();
}
public class QuestionTrueFalseAnswerEntity
{
    public Guid QuestionId { get; set; }

    public string QuestionType { get; set; } = string.Empty;

    public bool Answer { get; set; }
}
public class QuestionShortAnswerAnswerEntity
{
    public Guid QuestionId { get; set; }

    public string QuestionType { get; set; } = string.Empty;

    public string Answer { get; set; } = string.Empty;
}

