using Quiz.DbMigration.Abstraction;

namespace Quiz.WebAPI.ExtensionServices;

public static class DatabaseMirationHelper
{
    public static int MigrateDatabase(IServiceProvider services)
    {
        var migrationHelper = services.GetRequiredService<IUpgradeService>();

        return migrationHelper.UpgradeDatabase();
    }
}
