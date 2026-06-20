using Quiz.DL.Entities;

using Quiz.DL.Entities;
using Quiz.Shared.DTO.Question.Response;

namespace Quiz.DL.Abstractions;
public interface IQuestionRepository
{
    Task CreateQuestions(QuestionEntity question);

    Task UpdateQuestions(QuestionEntity question);

    Task SubmitAnswers(QuestionSubmissionEntity submission);

    Task<QuestionResponseDto> GetQuestions(Guid groupId);
}
