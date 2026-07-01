using Quiz.DL.Entities;
using Quiz.Shared.DTO.Teacher.Response;

namespace Quiz.DL.Abstractions;
public interface ITeacherRepository
{
    Task<IEnumerable<ExamReviewResponseDto>> GetExamsToBeReviewed(Guid userId);

    Task<IEnumerable<ShortAnswerQuestionsResponseDto>> GetShortAnswerQuestionsForReview(
        Guid groupId,
        Guid studentId);

    Task SubmitReviewResult(ReviewResultEntity reqEntity, Guid userId);
}
