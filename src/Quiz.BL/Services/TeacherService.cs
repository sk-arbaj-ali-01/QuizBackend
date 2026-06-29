using Microsoft.Extensions.Logging;
using Quiz.BL.Abstractions;
using Quiz.DL.Abstractions;
using Quiz.Shared.DTO.Teacher.Response;

namespace Quiz.BL.Services;
public class TeacherService(
    ITeacherRepository teacherRepository,
    ILogger<TeacherService> logger)
    : BaseService, ITeacherService
{
    public async Task<IEnumerable<ExamReviewResponseDto>> GetExamsToBeReviewed(Guid userId)
    {
        return await teacherRepository.GetExamsToBeReviewed(userId);
    }

    public async Task<IEnumerable<ShortAnswerQuestionsResponseDto>> GetShortAnswerQuestionsForReview(
        Guid groupId,
        Guid studentId)
    {
        return await teacherRepository.GetShortAnswerQuestionsForReview(groupId, studentId);
    }
}
