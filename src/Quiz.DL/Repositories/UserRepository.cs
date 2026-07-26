using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quiz.DL.Abstractions;
using Quiz.DL.Entities;
using Quiz.DL.Parameters;
using Quiz.DL.Service;
using Quiz.DL.SQLQueries;
using Quiz.Shared.DTO.User.Response;
using Quiz.Shared.Helpers;
using Quiz.Shared.Models;
using System.Text;
using System.Data;

namespace Quiz.DL.Repositories;
public class UserRepository(
    IConfiguration configuration,
    ILogger<UserRepository> logger)
    : DbConnectionManager(configuration, logger), IUserRepository
{
    public async Task CreateUser(UserEntity entity)
    {
        await DbOperation(async connection =>
            await connection.ExecuteAsync(
                UserSqlQueries.CreateUser,
                entity));
    }

    public async Task<UserResponseDto?> GetUserById(Guid userId)
    {
        UserResponseDto? response = await DbOperation(async connection =>
            await connection.QueryFirstOrDefaultAsync<UserResponseDto>(
                UserSqlQueries.GetUserById,
                new
                {
                    UserId = userId
                }));

        return response;
    }

    public async Task<UserLoginResponseDto?> Login(UserLoginEntity entity)
    {
        var response = await DbOperation(async connection =>
            await connection.QueryFirstOrDefaultAsync<UserLoginResponseDto>(
                UserSqlQueries.GetUserDeatilsByEmail,
                new
                {
                    Email = entity.Email.ToString()
                })
            );

        return response;
    }

    public async Task<PagedRecordModel<UserTeacherResponseDto>> GetTeachersData( 
        RelatedTeachersParameter parameter,
        Guid userId)
    {
        StringBuilder sqlBuilder = new(UserSqlQueries.GetTeachersData);
        DynamicParameters dynamicParameters = new DynamicParameters();

        dynamicParameters.Add("StudentId", userId, DbType.Guid);

        if (Utility.IsValidString(parameter.SearchQuery))
        {
            sqlBuilder.Append(
                @$" AND U.full_name LIKE @{nameof(parameter.SearchQuery)} ");
            dynamicParameters.Add(nameof(parameter.SearchQuery), $"%{parameter.SearchQuery}%");
        }

        if(parameter.PerPage > 0 && parameter.Page > 0)
        {
            sqlBuilder.Append(
                @$" ORDER BY U.user_id
                    LIMIT {parameter.PerPage}
                    OFFSET {parameter.PerPage * (parameter.Page - 1)}
            ");
        }
        else
        {
            sqlBuilder.Append(@" ORDER BY U.user_id ");
        }

        IEnumerable<UserTeacherResponseDto> results =
            await DbOperation(async connection =>
                await connection.QueryAsync<UserTeacherResponseDto>(
                    sqlBuilder.ToString(),
                    dynamicParameters
                )
            );

        return new PagedRecordModel<UserTeacherResponseDto>
        {
            Records = results,
            TotalCount = results.Count()
        };
    }

    public async Task CreateRelationBetweenStudentAndTeacher(Guid studentId, Guid teacherId)
    {
        await DbOperation(async conn =>
            await conn.ExecuteAsync(
                UserSqlQueries.CreateStudentAndTeacherData,
                new
                {
                    StudentId = studentId,
                    TeacherId = teacherId
                })
            );
    }

    public async Task<bool> CheckIfStudentAndTeacherDataAlreadyExists(Guid studentId, Guid teacherId)
    {
        return await DbOperation(async conn =>
            await conn.QueryFirstOrDefaultAsync<bool>(
                UserSqlQueries.CheckIfStudentAndTeacherDataAlreadyExists,
                new
                {
                    StudentId = studentId,
                    TeacherId = teacherId
                })
            );
    }
}
