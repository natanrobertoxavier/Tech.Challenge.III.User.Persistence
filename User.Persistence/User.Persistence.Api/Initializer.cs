using RabbitMq.Package.Settings;
using Serilog;
using TokenService.Manager.Controller;
using User.Persistence.Infrastructure.Queue;

namespace User.Persistence.Api;

public static class Initializer
{
    public static void AddTokenManager(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        AddJWTToken(services, configuration);
        AddSerilog(services);
        AddRabbitMqService(services, configuration);
        AddRabbitMqSettings(services, configuration);
    }

    private static void AddJWTToken(IServiceCollection services, IConfiguration configuration)
    {
        var sectionLifeTime = configuration.GetRequiredSection("Settings:Jwt:LifeTimeTokenMinutes");
        var sectionKey = configuration.GetRequiredSection("Settings:Jwt:KeyToken");
        services.AddScoped(option => new TokenController(int.Parse(sectionLifeTime.Value), sectionKey.Value));
    }

    private static void AddSerilog(IServiceCollection services)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        services.AddSingleton<Serilog.ILogger>(Log.Logger);
    }

    private static void AddRabbitMqService(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMqSettings"));
    }

    private static void AddRabbitMqSettings(IServiceCollection services, IConfiguration configuration)
    {
        var config = new RabbitMqSettings();

        configuration.GetSection("RabbitMqSettings").Bind(config);

        services
            .AddQueueHandler(config.ComposedConnectionString)
            .DeclareQueues(
                new RabbitMqQueue(
                    exchangeName: RabbitMqConstants.UserPersistenceExchange,
                    routingKeyName: RabbitMqConstants.RegisterUserRoutingKey,
                    queueName: RabbitMqConstants.RegisterUserQueueName)
                )
            ;
    }
}
