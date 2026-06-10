using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using System.Data;

namespace Quiz.DL.Service;
internal abstract class DbConnectionManager
{
    private IConfiguration _configuration;
    private ILogger<DbConnectionManager> _logger;

    protected DbConnectionManager(
        IConfiguration configuration,
        ILogger<DbConnectionManager> logger)
    {
        _configuration = configuration; 
        _logger = logger;
    }

    public async Task<T> DbOperation<T>(Func<IDbConnection, Task<T>> dbOperation)
    {
        return await ExecuteOperation<T>(dbOperation);
    }

    public async Task<T> DbOperationInTransaction<T>(Func<IDbConnection, IDbTransaction, Task<T>> dbOperation)
    {
        return await ExecuteOperation<T>(async (connection) =>
        {
            using IDbTransaction transaction = connection.BeginTransaction();

            try
            {
                T result = await dbOperation(connection, transaction);
                transaction.Commit();
                return result;
            }
            catch(Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "There is an database or transaction error : {ex}", ex.Message);
                throw;
            }
        });
    }

    private async Task<T> ExecuteOperation<T>(Func<IDbConnection, Task<T>> dbOperation)
    {
        using IDbConnection connection = GetConnection();

        try
        {
            connection.Open();

            return await dbOperation(connection);
        }
        catch (Exception ex)
        {
            _logger.LogInformation("Database execution failed : {ex}", ex.Message);
            throw;
        }
    }

    private IDbConnection GetConnection()
    {
        return new MySqlConnection(GetConnectionString());
    }

    private string GetConnectionString()
    {
        string? connectionString = _configuration.GetConnectionString("DefaultConnection");

        return ValidateConnectionString(connectionString);
    }

    private string ValidateConnectionString(string? connectionString)
    {
        if(string.IsNullOrWhiteSpace(connectionString))
        {
            _logger.LogInformation("Database connection string should be a valid value");
            throw new Exception("Database connection string should be a valid value");
        }

        return connectionString;
    }
}
