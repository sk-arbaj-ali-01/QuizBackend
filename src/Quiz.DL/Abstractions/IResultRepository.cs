using Quiz.Shared.DTO.Result.Response;

namespace Quiz.DL.Abstractions;
public interface IResultRepository
{
    Task<ExamQuestionsResponseDto> GetExamQuestionsByGroupId(Guid groupId);

    Task<StudentAnswersResponseDto> GetStudentAnswersByUserAndGroupId(
        Guid userId,
        Guid groupId,
        IEnumerable<Guid> mcqQuestionIds,
        IEnumerable<Guid> msqQuestionIds,
        IEnumerable<Guid> tfQuestionIds,
        IEnumerable<Guid> saQuestionIds);
}
