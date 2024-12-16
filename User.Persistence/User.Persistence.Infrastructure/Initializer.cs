using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using User.Persistence.Domain.Extensions;
using User.Persistence.Domain.Repositories;
using User.Persistence.Domain.Repositories.User;
using User.Persistence.Domain.Services;
using User.Persistence.Infrastructure.Queue;
using User.Persistence.Infrastructure.RepositoryAccess;
using User.Persistence.Infrastructure.RepositoryAccess.User;
using User.Persistence.Infrastructure.ServicesAccess;

namespace User.Persistence.Infrastructure;
public static class Initializer
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configurationManager)
    {
        AddFluentMigrator(services, configurationManager);
        AddContext(services, configurationManager);
        AddServices(services);
        AddRabbitMqDispatcher(services);
        AddWorkUnit(services);
        RegisterServices(services, configurationManager);
        AddRepositories(services);
    }

    private static void AddWorkUnit(IServiceCollection services)
    {
        services.AddScoped<IWorkUnit, WorkUnit>();
    }

    private static void AddFluentMigrator(IServiceCollection services, IConfiguration configurationManager)
    {
        _ = bool.TryParse(configurationManager.GetSection("Settings:DatabaseInMemory").Value, out bool databaseInMemory);

        if (!databaseInMemory)
        {
            services.AddFluentMigratorCore().ConfigureRunner(c =>
                 c.AddMySql5()
                  .WithGlobalConnectionString(configurationManager.GetFullConnection())
                  .ScanIn(Assembly.Load("User.Persistence.Infrastructure")).For.All());
        }
    }

    private static void AddContext(IServiceCollection services, IConfiguration configurationManager)
    {
        _ = bool.TryParse(configurationManager.GetSection("Settings:DatabaseInMemory").Value, out bool databaseInMemory);

        if (!databaseInMemory)
        {
            var versaoServidor = new MySqlServerVersion(new Version(8, 0, 26));
            var connectionString = configurationManager.GetFullConnection();

            services.AddDbContext<TechChallengeContext>(dbContextoOpcoes =>
            {
                dbContextoOpcoes.UseMySql(connectionString, versaoServidor);
            });
        }
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddScoped<IUserQueryServiceApi, UserQueryServiceApi>();
    }

    private static void AddRabbitMqDispatcher(IServiceCollection services)
    {
        services.AddScoped<IRabbitMqEventsDispatcher, RabbitMqEventsDispatcher>();
    }

    private static void RegisterServices(IServiceCollection services, IConfiguration configurationManager)
    {
        services.AddHttpClient("UserQueryApi", client =>
        {
            client.BaseAddress = new Uri(configurationManager.GetSection("ServicesApiAddress:UserQueryApi").Value);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();
    }
}
