using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quiz.DL.Abstractions;
using Quiz.DL.Service;
using Quiz.DL.SQLQueries;
using Quiz.Shared.DTO.Student.Response;

namespace Quiz.DL.Repositories;
public class StudentRepository(
    IConfiguration configuration,
    ILogger<StudentRepository> logger)
    : DbConnectionManager(configuration, logger), IStudentRepository
{
    public async Task<IEnumerable<GroupsForStudentsResponseDto>> GetGroupsFroStudents(Guid studentId)
    {
        IEnumerable<GroupsForStudentsResponseDto> response =
            await DbOperation(async conn =>
                await conn.QueryAsync<GroupsForStudentsResponseDto>(
                    StudentSqlQueries.GetGroupsForStudents,
                    new
                    {
                        UserId = studentId
                    })
                );

        return response;
    }
}
