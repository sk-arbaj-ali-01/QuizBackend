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
}
