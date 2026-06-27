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
            McqAnswers = requestDto.McqAnswers.Select(x => new QuestionMcqAnswerEntity
            {
                OptionId = x.OptionId,
                QuestionId = x.QuestionId,
                QuestionType = x.QuestionType.ToString(),
            }).ToList(),
            MsqAnswers = requestDto.MsqAnswers.Select(x => new QuestionMsqAnswerEntity
            {
                OptionIds = x.OptionIds,
                QuestionId = x.QuestionId,
                QuestionType = x.QuestionType.ToString(),
            }).ToList(),
            TrueFalseAnswers = requestDto.TrueFalseAnswers.Select(x => new QuestionTrueFalseAnswerEntity
            {
                Answer = x.Answer,
                QuestionId = x.QuestionId,
                QuestionType = x.QuestionType.ToString(),
            }).ToList(),
            ShortAnswers = requestDto.ShortAnswers.Select(x => new QuestionShortAnswerAnswerEntity
            {
                Answer = x.Answer,
                QuestionId = x.QuestionId,
                QuestionType = x.QuestionType.ToString(),
            }).ToList(),
        };
    }
}
