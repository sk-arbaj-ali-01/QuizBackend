using Quiz.Shared.DTO.Teacher.Request;
using Quiz.Shared.DTO.Teacher.Response;

namespace Quiz.BL.Abstractions;
public interface ITeacherService
{
    Task<IEnumerable<ExamReviewResponseDto>> GetExamsToBeReviewed(Guid userId);

    Task<IEnumerable<ShortAnswerQuestionsResponseDto>> GetShortAnswerQuestionsForReview(
        Guid groupId,
        Guid studentId);

    Task SubmitReviewResult(ReviewResultRequestDto reqDto, Guid userId);
}
