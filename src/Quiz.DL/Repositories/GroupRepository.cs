using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quiz.DL.Abstractions;
using Quiz.DL.Entities;
using Quiz.DL.Service;
using Quiz.DL.SQLQueries;

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
}
