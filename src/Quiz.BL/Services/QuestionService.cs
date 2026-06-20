using Microsoft.Extensions.Logging;
using Quiz.BL.Abstractions;
using Quiz.BL.DtoToEntityExtensions.Question;
using Quiz.DL.Abstractions;
using Quiz.DL.Entities;
using Quiz.Shared.DTO.Question.Request;
using Quiz.Shared.DTO.Question.Response;

namespace Quiz.BL.Services;
public class QuestionService(
    ILogger<QuestionService> logger,
    IQuestionRepository questionRepository) : BaseService, IQuestionService
{
    public async Task CreateQuestions(QuestionCreateRequestDto question)
    {
        QuestionEntity questionEntity = question.ConvertToEntity();

        await questionRepository.CreateQuestions(questionEntity);
    }

    public async Task UpdateQuestions(QuestionUpdateRequestDto question)
    {
        QuestionEntity questionEntity = question.ConvertToEntity();

        await questionRepository.UpdateQuestions(questionEntity);
    }

    public async Task SubmitAnswers(QuestionSubmitAnswersRequestDto requestDto, Guid userId)
    {
        QuestionSubmissionEntity submission = requestDto.ConvertToEntity(userId);

        await questionRepository.SubmitAnswers(submission);
    }

    public async Task<QuestionResponseDto> GetQuestions(Guid groupId)
    {
        return await questionRepository.GetQuestions(groupId);
    }
}
