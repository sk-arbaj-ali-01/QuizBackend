namespace Quiz.DL.Entities;
public class ReviewResultEntity : BaseEntity
{
    public Guid GroupId { get; set; }

    public Guid StudentId { get; set; }

    public IEnumerable<StudentReviewDataEntity> Submission { get; set; } = new List<StudentReviewDataEntity>();
}

public class StudentReviewDataEntity
{
    public Guid QuestionId { get; set; }

    public bool IsCorrect { get; set; }
}
