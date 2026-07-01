using Quiz.DL.Entities;
using Quiz.Shared.DTO.Teacher.Request;

namespace Quiz.BL.DtoToEntityExtensions.Teacher;
public static class ReviewResultDtoToReviewResultEntity
{
    public static ReviewResultEntity ConvertToEntity(this ReviewResultRequestDto reqDto)
    {
        return new ReviewResultEntity
        {
            GroupId = reqDto.GroupId,
            StudentId = reqDto.StudentId,
            Submission = reqDto.Submission.Select(x => new StudentReviewDataEntity
            {
                QuestionId = x.QuestionId,
                IsCorrect = x.IsCorrect,
            })
        };
    }
}
