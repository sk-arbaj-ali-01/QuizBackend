namespace Quiz.DL.Entities;
public class QuestionSubmissionEntity
{
    public Guid GroupId { get; set; }

    public Guid UserId { get; set; }

    public List<QuestionAnswerEntity> Questions { get; set; } = new();
}

public class QuestionAnswerEntity
{
    public Guid QuestionId { get; set; }

    public string QuestionType { get; set; } = string.Empty;

    public Guid? OptionId { get; set; }
}
