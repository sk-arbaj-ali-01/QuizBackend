using Quiz.Shared.DTO.Teacher.Response;

namespace Quiz.BL.Abstractions;
public interface ITeacherService
{
    Task<IEnumerable<ExamReviewResponseDto>> GetExamsToBeReviewed(Guid userId);
}
