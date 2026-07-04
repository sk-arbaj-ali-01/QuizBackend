using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quiz.DL.Abstractions;
using Quiz.DL.Service;
using Quiz.DL.SQLQueries;
using Quiz.Shared.DTO.Result.Response;

namespace Quiz.DL.Repositories;
public class ResultRepository(
    IConfiguration configuration,
    ILogger<ResultRepository> logger)
    : DbConnectionManager(configuration, logger), IResultRepository
{
    public async Task<ExamQuestionsResponseDto> GetExamQuestionsByGroupId(Guid groupId)
    {
        ExamQuestionsResponseDto examQuestionsResponseDto = new();

        examQuestionsResponseDto.GroupId = groupId;

        Dictionary<Guid, ExamMcqQuestionsResponseDto> _examMcqQuestions = new();

        await DbOperation(async conn =>
            await conn.QueryAsync<
                ExamMcqQuestionsResponseDto,
                ExamMcqQuestionOptionsResponseDto,
                ExamMcqQuestionsResponseDto>(
                ResultSqlQueries.GetMcqQuestionAndOptionsByGroupId,
                (mcq, option) =>
                {
                    if(!_examMcqQuestions.TryGetValue(mcq.QuestionId, out var entry))
                    {
                        entry = mcq;
                        entry.Options = new List<ExamMcqQuestionOptionsResponseDto>();
                        _examMcqQuestions[mcq.QuestionId] = entry;
                    }

                    entry.Options = entry.Options.Append(option);

                    return mcq;
                },
                new
                {
                    GroupId = groupId,
                },
                splitOn: "OptionId"));

        examQuestionsResponseDto.McqQuestions = _examMcqQuestions.Values;

        Dictionary<Guid, ExamMsqQuestionsResponseDto> _examMsqQuestions = new();

        await DbOperation(async conn =>
            await conn.QueryAsync<
                ExamMsqQuestionsResponseDto,
                ExamMcqQuestionOptionsResponseDto,
                ExamMsqQuestionsResponseDto>(
                ResultSqlQueries.GetMsqQuestionAndOptionsByGroupId,
                (mcq, option) =>
                {
                    if(!_examMcqQuestions.TryGetValue(mcq.QuestionId, out var entry))
                    {
                        entry = mcq;
                        entry.Options = new List<ExamMcqQuestionOptionsResponseDto>();
                        _examMcqQuestions[mcq.QuestionId] = entry;
                    }

                    entry.Options = entry.Options.Append(option);

                    return mcq;
                },
                new
                {
                    GroupId = groupId,
                },
                splitOn: "OptionId"));

        examQuestionsResponseDto.MsqQuestions = _examMsqQuestions.Values;

        examQuestionsResponseDto.TrueFalseQuestions =
            await DbOperation(async conn =>
                await conn.QueryAsync<ExamTrueFalseQuestionsResponseDto>(
                    ResultSqlQueries.GetTrueFalseQuestionsByGroupId,
                    new
                    {
                        GroupId = groupId,
                    }));

        examQuestionsResponseDto.ShortAnswerQuestions =
            await DbOperation(async conn =>
                await conn.QueryAsync<ExamShortAnswerQuestionsResponseDto>(
                    ResultSqlQueries.GetShortAnswerQuestionsByGroupId,
                    new
                    {
                        GroupId = groupId,
                    }));

        return examQuestionsResponseDto;
                
    }

    public async Task<StudentAnswersResponseDto> GetStudentAnswersByUserAndGroupId(
        Guid userId, 
        Guid groupId,
        IEnumerable<Guid> mcqQuestionIds,
        IEnumerable<Guid> msqQuestionIds,
        IEnumerable<Guid> tfQuestionIds,
        IEnumerable<Guid> saQuestionIds)
    {
        StudentAnswersResponseDto studentAnswers = new();
        studentAnswers.GroupId = groupId;

        studentAnswers.McqAnswers = await DbOperation(async conn =>
            await conn.QueryAsync<StudentMcqAnswersResponseDto>(
                ResultSqlQueries.GetMcqOrMsqSubmissionDataByUserId,
                new
                {
                    UserId = userId,
                    QuestionIds = mcqQuestionIds.ToArray(),
                }));

        Dictionary<Guid, StudentMsqAnswersResponseDto> _answerDict = new();

        await DbOperation(async conn =>
            await conn.QueryAsync<
                StudentMsqAnswersResponseDto,
                Guid,
                StudentMsqAnswersResponseDto>(
                ResultSqlQueries.GetMcqOrMsqSubmissionDataByUserId,
                (msq, option) =>
                {
                    if(!_answerDict.TryGetValue(msq.QuestionId, out var entry))
                    {
                        entry = msq;
                        entry.OptionIds = new List<Guid>();
                        _answerDict[msq.QuestionId] = entry;
                    }

                    entry.OptionIds = entry.OptionIds.Append(option);

                    return msq;
                },
                new
                {
                    UserId = userId,
                    QuestionIds = msqQuestionIds,
                },
                splitOn: "OptionId"));

        studentAnswers.MsqAnswers = _answerDict.Values;

        studentAnswers.TrueFalseAnswers = await DbOperation(async conn =>
            await conn.QueryAsync<StudentTrueFalseAnswersResponseDto>(
                ResultSqlQueries.GetTrueFalseSubmissionDataByUserId,
                new
                {
                    UserId = userId,
                    QuestionIds = tfQuestionIds
                }));

        studentAnswers.ShortAnswers = await DbOperation(async conn =>
            await conn.QueryAsync<StudentShortAnswersResponseDto>(
                ResultSqlQueries.GetShortAnswerSubmissionDataByUserId,
                new
                {
                    UserId = userId,
                    QuestionIds = saQuestionIds
                }));

        return studentAnswers;
    }
}
