using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TokenService.Manager.Controller;
using User.Persistence.Application.Interfaces;
using User.Persistence.Application.Messages;
using User.Persistence.Application.Messages.Handlers;
using User.Persistence.Application.Services;
using User.Persistence.Application.Services.LoggedUser;
using User.Persistence.Application.UseCase.ChangePassword;
using User.Persistence.Application.UseCase.Register;
using User.Persistence.Domain.Messages;
using User.Persistence.Domain.Messages.DomaiEvents;

namespace User.Persistence.Application;

public static class Initializer
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        AddAdditionalKeyPassword(services, configuration);
        AddJWTToken(services, configuration);
        AddUseCases(services);
        AddLoggedUser(services);
        AddDomainEvents(services);
        AddEvents(services);
    }

    private static void AddAdditionalKeyPassword(IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetRequiredSection("Settings:Password:AdditionalKeyPassword");
        services.AddScoped(option => new PasswordEncryptor(section.Value));
    }

    private static void AddJWTToken(IServiceCollection services, IConfiguration configuration)
    {
        var sectionLifeTime = configuration.GetRequiredSection("Settings:Jwt:LifeTimeTokenMinutes");
        var sectionKey = configuration.GetRequiredSection("Settings:Jwt:KeyToken");
        services.AddScoped(option => new TokenController(int.Parse(sectionLifeTime.Value), sectionKey.Value));
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();
    }
    private static void AddLoggedUser(IServiceCollection services)
    {
        services.AddScoped<ILoggedUser, LoggedUser>();
    }

    private static void AddDomainEvents(IServiceCollection services)
    {
        services.AddScoped<IMessagePublisher, MessagePublisher>();
        services.AddScoped<INotificationHandler<UserCreateDomainEvent>, UserEventHandler>();
    }

    private static void AddEvents(IServiceCollection services)
    {
        services.AddScoped<IEventAppService, EventsAppService>();
    }
}
