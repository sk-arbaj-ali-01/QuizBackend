using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quiz.DL.Abstractions;
using Quiz.DL.Service;
using Quiz.DL.SQLQueries;
using Quiz.Shared.DTO.Teacher.Response;

namespace Quiz.DL.Repositories;
public class TeacherRepository(
    IConfiguration configuration,
    ILogger<TeacherRepository> logger)
    : DbConnectionManager(configuration, logger), ITeacherRepository
{
    public async Task<IEnumerable<ExamReviewResponseDto>> GetExamsToBeReviewed(Guid userId)
    {
        return await DbOperation(async conn =>
            await conn.QueryAsync<ExamReviewResponseDto>(
                TeacherSqlQueries.GetExamDataToBeReviewed,
                new
                {
                    UserId = userId
                }));
    }

    public async Task<IEnumerable<ShortAnswerQuestionsResponseDto>> GetShortAnswerQuestionsForReview(
        Guid groupId,
        Guid studentId)
    {
        IEnumerable<ShortAnswerQuestionsResponseDto> questions =
            await DbOperation(async conn =>
                await conn.QueryAsync<ShortAnswerQuestionsResponseDto>(
                    TeacherSqlQueries.GetShortAnswerQuestionByGroupId,
                    new
                    {
                        GroupId = groupId
                    }));

        IEnumerable<ShortAnswerResponseDto> answers =
            await DbOperation(async conn =>
                await conn.QueryAsync<ShortAnswerResponseDto>(
                    TeacherSqlQueries.GetShortAnswersByStudentId,
                    new
                    {
                        QuestionIds = questions.Select(x => x.QuestionId),
                        StudentId = studentId
                    }));

        List<ShortAnswerQuestionsResponseDto> answersForReview = new();

        foreach(var item in answers)
        {
            foreach(var question in questions)
            {
                if(item.QuestionId == question.QuestionId)
                {
                    question.AnswerText = item.AnswerText;
                    answersForReview.Add(question);
                }
            }
        }

        return answersForReview;
    }
}
