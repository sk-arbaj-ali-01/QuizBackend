using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quiz.DL.Abstractions;
using Quiz.DL.Entities;
using Quiz.DL.Parameters;
using Quiz.DL.Service;
using Quiz.DL.SQLQueries;
using Quiz.Shared.DTO.Group.Response;
using Quiz.Shared.Helpers;
using Quiz.Shared.Models;
using System.Text;

namespace Quiz.DL.Repositories;
public class GroupRepository(
    IConfiguration config,
    ILogger<GroupRepository> logger) 
    : DbConnectionManager(
        config, logger), IGroupRepository
{
    public async Task CreateGroup(GroupEntity entity)
    {
        await DbOperation(async (connection) =>
            await connection.ExecuteAsync(
                GroupSqlQueries.CreateGroupQuery,
                entity));
    }

    public async Task<PagedRecordModel<GroupResponseDto>> GetGroups(GroupParameter parameter)
    {
        DynamicParameters dynamicParameters;
        string sqlQuery = BuildQuery(parameter, out dynamicParameters);

        IEnumerable<GroupResponseDto> results =
            await DbOperation(async conn =>
                await conn.QueryAsync<GroupResponseDto>(
                    sqlQuery,
                    dynamicParameters));

        return new PagedRecordModel<GroupResponseDto>
        {
            Records = results,
            TotalCount = results.Count()
        };
    }

    public async Task<GroupResponseDto> GetGroupById(Guid groupId)
    {
        GroupResponseDto response =
            await DbOperation(async conn =>
                await conn.QuerySingleAsync<GroupResponseDto>(
                    GroupSqlQueries.GetGroupByIdQuery,
                    new
                    {
                        GroupId = groupId
                    }));

        return response;
    }

    public async Task UpdateGroupById(GroupEntity groupEntity)
    {
        await DbOperation(async conn =>
            await conn.ExecuteAsync(
                GroupSqlQueries.UpdateGroupById,
                groupEntity));
    }

    public async Task DeleteGroupById(Guid groupId)
    {
        await DbOperation(async conn =>
            await conn.ExecuteAsync(
                GroupSqlQueries.DeleteGroupById,
                new
                {
                    GroupId = groupId
                }));
    }

    private string BuildQuery(GroupParameter parameter, out DynamicParameters dynamicParameters)
    {
        StringBuilder sqlBuilder = new StringBuilder(GroupSqlQueries.GetGroupsQuery);
        dynamicParameters = new DynamicParameters();
        bool andRequired = false;

        if(Utility.IsValidString(parameter.SearchQuery) ||
            parameter.IsActive.HasValue ||
            parameter.IsArchieved.HasValue)
        {
            sqlBuilder.Append(" WHERE ");
        }

        if(Utility.IsValidString(parameter.SearchQuery))
        {
            sqlBuilder.Append($" group_name LIKE @{nameof(parameter.SearchQuery)}");
            dynamicParameters.Add(nameof(parameter.SearchQuery), $"%{parameter.SearchQuery}%");

            andRequired = true;
        }

        if (parameter.IsActive.HasValue)
        {
            if(andRequired)
            {
                sqlBuilder.Append(" AND ");
            }
            sqlBuilder.Append(@$" is_active = @{nameof(parameter.IsActive)} ");
            dynamicParameters.Add(nameof(parameter.IsActive), parameter.IsActive.Value);

            andRequired = true;
        }

        if (parameter.IsArchieved.HasValue)
        {
            if(andRequired)
            {
                sqlBuilder.Append(" AND ");
            }
            sqlBuilder.Append(@$" is_archived = @{nameof(parameter.IsArchieved)} ");
            dynamicParameters.Add(nameof(parameter.IsArchieved), parameter.IsArchieved.Value);
        }

        sqlBuilder.Append(@" ORDER BY created_at DESC ");

        sqlBuilder.Append(@$"
            LIMIT {parameter.PerPage}
            OFFSET {parameter.PerPage * (parameter.Page - 1)}
        ");

        return sqlBuilder.ToString();
    }
}
