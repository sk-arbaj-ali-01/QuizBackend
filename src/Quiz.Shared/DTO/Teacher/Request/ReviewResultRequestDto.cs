namespace Quiz.Shared.DTO.Teacher.Request;
public class ReviewResultRequestDto
{
    public Guid GroupId { get; set; }

    public Guid StudentId { get; set; }

    public IEnumerable<StudentReviewDataRequestDto> Submission { get; set; } = new List<StudentReviewDataRequestDto>();
}

public class StudentReviewDataRequestDto
{
    public Guid QuestionId { get; set; }

    public bool IsCorrect { get; set; }
}