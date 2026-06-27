using Microsoft.AspNetCore.Authentication.JwtBearer;
using Quiz.BL.Abstractions;
using Quiz.BL.Services;
using Quiz.DbMigration.Abstraction;
using Quiz.DbMigration.Service;
using Quiz.DL.Abstractions;
using Quiz.DL.Repositories;
using Quiz.Shared.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

namespace Quiz.WebAPI.ExtensionServices;

public static class ServiceRegisterExtensions
{
    public static IServiceCollection AddServicesToCollection(
        this IServiceCollection services, WebApplicationBuilder builder)
    {
        services
            .AddHelperServices()
            .AddBusinessLayerServices()
            .AddDatabaseLayerRepositories()
            .AddCorsPolicies()
            .AddPoliciesAndOptions(builder)
            .AddAuthenticationServices(builder)
            .AddAuthorizationService();

        return services;
    }

    private static IServiceCollection AddBusinessLayerServices(
        this IServiceCollection services)
    {
        services.AddScoped<IGroupService, GroupService>();
        services.AddScoped<IQuestionService, QuestionService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<ITeacherService, TeacherService>();

        return services;
    }

    private static IServiceCollection AddDatabaseLayerRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<IGroupRepository, GroupRepository>();
        services.AddScoped<IQuestionRepository, QuestionRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<ITeacherRepository, TeacherRepository>();

        return services;
    }

    private static IServiceCollection AddHelperServices(
        this IServiceCollection services)
    {
        services.AddSingleton<IUpgradeService ,DbMigrationService>();

        return services;
    }

    private static IServiceCollection AddPoliciesAndOptions(this IServiceCollection services, WebApplicationBuilder builder)
    {
        builder.Services.Configure<JWTOptions>(
            builder.Configuration.GetSection("JWTOptions"));

        return services;
    }

    private static IServiceCollection AddCorsPolicies(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });
        });

        return services;
    }

    private static IServiceCollection AddAuthenticationServices(
        this IServiceCollection services,
        WebApplicationBuilder builder)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Audience = builder.Configuration.GetValue<string>("JWTOptions:Audience");
                //options.Authority = builder.Configuration.GetValue<string>("JWTOptions:Issuer");
                //options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JWTOptions:Key")!)),
                    ValidIssuer = builder.Configuration.GetValue<string>("JWTOptions:Issuer"),
                    ValidAudience = builder.Configuration.GetValue<string>("JWTOptions:Audience"),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(30)
                };
            });
        return services;
    }

    private static IServiceCollection AddAuthorizationService(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("Teacher", policy =>
            {
                policy.RequireClaim(ClaimTypes.Role, "TEACHER");
            });
            options.AddPolicy("Student", policy =>
            {
                policy.RequireClaim(ClaimTypes.Role, "STUDENT");
            });
        });

        return services;
    }
}
