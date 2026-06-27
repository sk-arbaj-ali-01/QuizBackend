using Quiz.Shared.Enums;

namespace Quiz.Shared.DTO.Result.Response;
public class ExamQuestionsResponseDto
{
    public Guid GroupId { get; set; }

    public IEnumerable<ExamMcqQuestionsResponseDto> McqQuestions { get; set; } = new List<ExamMcqQuestionsResponseDto>();

    public IEnumerable<ExamMsqQuestionsResponseDto> MsqQuestions { get; set; } = new List<ExamMsqQuestionsResponseDto>();

    public IEnumerable<ExamTrueFalseQuestionsResponseDto> TrueFalseQuestions { get; set; } = new List<ExamTrueFalseQuestionsResponseDto>();

    public IEnumerable<ExamShortAnswerQuestionsResponseDto> ShortAnswerQuestions { get; set; } = new List<ExamShortAnswerQuestionsResponseDto>();
}

public class ExamMcqQuestionsResponseDto
{
    public Guid QuestionId { get; set; }

    public string QuestionText { get; set; } = string.Empty;

    public QuestionTypeEnum QuestionType { get; set; }

    public int Points { get; set; }

    public IEnumerable<ExamMcqQuestionOptionsResponseDto> Options { get; set; } = new List<ExamMcqQuestionOptionsResponseDto>();
}

public class ExamMcqQuestionOptionsResponseDto
{
    public Guid OptionId { get; set; }

    public string OptionText { get; set; } = string.Empty;

    public bool IsCorrect { get; set; }
}

public class ExamMsqQuestionsResponseDto : ExamMcqQuestionsResponseDto
{}

public class ExamTrueFalseQuestionsResponseDto
{
    public Guid QuestionId { get; set; }

    public string QuestionText { get; set; } = string.Empty;

    public QuestionTypeEnum QuestionType { get; set; }

    public int Points { get; set; }

    public bool CorrectAnswer { get; set; }
}

public class ExamShortAnswerQuestionsResponseDto
{
    public Guid QuestionId { get; set; }

    public string QuestionText { get; set; } = string.Empty;

    public QuestionTypeEnum QuestionType { get; set; }

    public int Points { get; set; }
}