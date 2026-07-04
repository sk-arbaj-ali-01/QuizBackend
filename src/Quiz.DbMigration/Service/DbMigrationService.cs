using DbUp;
using DbUp.Engine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quiz.DbMigration.Abstraction;
using System.Reflection;

namespace Quiz.DbMigration.Service;
public class DbMigrationService(
    ILogger<DbMigrationService> logger,
    IConfiguration configuration) : IUpgradeService
{
    public int UpgradeDatabase()
    {
        string connectionString = ValidateConnectionString();
        //EnsureDatabase.For.MySqlDatabase(connectionString);

        UpgradeEngine upgrader = DeployChanges.To
            .MySqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
            .LogToConsole()
            .Build();

        DatabaseUpgradeResult result = upgrader.PerformUpgrade();

        if(result.Successful)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Database migration successful");
            Console.ResetColor();
            return 1;
        }

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Database migration error : {error}", result.Error);
        Console.ResetColor();
        return 0;
    }

    private string ValidateConnectionString()
    {
        string? connectionString = configuration.GetConnectionString("DefaultConnection");

        if(string.IsNullOrWhiteSpace(connectionString))
        {
            logger.LogError("The database connection string is not present or have wrong value.");
            throw new Exception("The database connection string is not present or have wrong value.");
        }

        return connectionString;
    }
}
