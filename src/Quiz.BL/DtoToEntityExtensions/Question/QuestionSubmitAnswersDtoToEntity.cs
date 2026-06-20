using Quiz.DL.Entities;
using Quiz.Shared.DTO.Question.Request;

namespace Quiz.BL.DtoToEntityExtensions.Question;
public static class QuestionSubmitAnswersDtoToEntity
{
    public static QuestionSubmissionEntity ConvertToEntity(this QuestionSubmitAnswersRequestDto requestDto, Guid userId)
    {
        return new QuestionSubmissionEntity
        {
            GroupId = requestDto.GroupId,
            UserId = userId,
            Questions = requestDto.Questions.Select(x =>
                new QuestionAnswerEntity
                {
                    QuestionId = x.QuestionId,
                    QuestionType = x.QuestionType.ToString(),
                    OptionId = x.OptionId
                }).ToList()
        };
    }
}
