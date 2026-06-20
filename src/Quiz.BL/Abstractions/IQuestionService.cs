using Quiz.DL.Entities;
using Quiz.Shared.DTO.Question.Request;
using Quiz.Shared.DTO.Question.Response;

namespace Quiz.BL.Abstractions;
public interface IQuestionService
{
    Task CreateQuestions(QuestionCreateRequestDto question);

    Task UpdateQuestions(QuestionUpdateRequestDto question);

    Task SubmitAnswers(QuestionSubmitAnswersRequestDto requestDto, Guid userId);

    Task<QuestionResponseDto> GetQuestions(Guid groupId);
}
