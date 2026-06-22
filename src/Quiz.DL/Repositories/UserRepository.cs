using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quiz.DL.Abstractions;
using Quiz.DL.Entities;
using Quiz.DL.Service;
using Quiz.DL.SQLQueries;
using Quiz.Shared.DTO.User.Response;
using Quiz.Shared.Models;

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

    public async Task<PagedRecordModel<UserTeacherResponseDto>> GetTeachersData()
    {
        IEnumerable<UserTeacherResponseDto> results =
            await DbOperation(async connection =>
                await connection.QueryAsync<UserTeacherResponseDto>(
                    UserSqlQueries.GetTeachersData)
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
