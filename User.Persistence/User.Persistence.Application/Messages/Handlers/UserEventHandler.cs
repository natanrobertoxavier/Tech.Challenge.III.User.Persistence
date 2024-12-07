using MediatR;
using Serilog;
using User.Persistence.Application.Interfaces;
using User.Persistence.Domain.Messages.DomaiEvents;

namespace User.Persistence.Application.Messages.Handlers;
public class UserEventHandler(
    ILogger logger,
    IEventAppService eventAppService) : INotificationHandler<UserCreateDomainEvent>
{
    private readonly ILogger _logger = logger;
    private readonly IEventAppService _eventAppService = eventAppService;

    public async Task Handle(UserCreateDomainEvent message, CancellationToken cancellationToken)
    {
        await _eventAppService.SendUserCreateEvent(message);
    }
}
