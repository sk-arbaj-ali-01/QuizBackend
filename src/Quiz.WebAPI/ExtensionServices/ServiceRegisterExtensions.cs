using Quiz.BL.Abstractions;
using Quiz.BL.Abstractions;
using Quiz.BL.Services;
using Quiz.DbMigration.Abstraction;
using Quiz.DbMigration.Service;
using Quiz.DL.Abstractions;
using Quiz.DL.Repositories;

namespace Quiz.WebAPI.ExtensionServices;

public static class ServiceRegisterExtensions
{
    public static IServiceCollection AddServicesToCollection(
        this IServiceCollection services)
    {
        services
            .AddHelperServices()
            .AddBusinessLayerServices()
            .AddDatabaseLayerRepositories();

        return services;
    }

    private static IServiceCollection AddBusinessLayerServices(
        this IServiceCollection services)
    {
        services.AddScoped<IGroupService, GroupService>();
        services.AddScoped<IQuestionService, QuestionService>();

        return services;
    }

    private static IServiceCollection AddDatabaseLayerRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<IGroupRepository, GroupRepository>();
        services.AddScoped<IQuestionRepository, QuestionRepository>();

        return services;
    }

    private static IServiceCollection AddHelperServices(
        this IServiceCollection services)
    {
        services.AddSingleton<IUpgradeService ,DbMigrationService>();

        return services;
    }
}
