using Quiz.Shared.Enums;

namespace Quiz.Shared.DTO.Result.Response;
public class StudentAnswersResponseDto
{
    public Guid GroupId { get; set; }

    public IEnumerable<StudentMcqAnswersResponseDto> McqAnswers { get; set; } = new List<StudentMcqAnswersResponseDto>();

    public IEnumerable<StudentMsqAnswersResponseDto> MsqAnswers { get; set; } = new List<StudentMsqAnswersResponseDto>();

    public IEnumerable<StudentTrueFalseAnswersResponseDto> TrueFalseAnswers { get; set; } = new List<StudentTrueFalseAnswersResponseDto>();

    public IEnumerable<StudentShortAnswersResponseDto> ShortAnswers { get; set; } = new List<StudentShortAnswersResponseDto>();
}

public class StudentMcqAnswersResponseDto
{
    public Guid QuestionId { get; set; }

    public Guid OptionId { get; set; }

    public DateTime AnsweredAt { get; set; }
}

public class StudentMsqAnswersResponseDto
{
    public Guid QuestionId { get; set; }

    public IEnumerable<Guid> OptionIds { get; set; } = new List<Guid>();

    public DateTime AnsweredAt { get; set; }
}

public class StudentTrueFalseAnswersResponseDto
{
    public Guid QuestionId { get; set; }

    public bool Answer { get; set; }

    public DateTime AnsweredAt { get; set; }
}

public class StudentShortAnswersResponseDto
{
    public Guid QuestionId { get; set; }

    public string AnswerText { get; set; } = string.Empty;

    public bool IsCorrect { get; set; }

    public DateTime AnsweredAt { get; set; }
}