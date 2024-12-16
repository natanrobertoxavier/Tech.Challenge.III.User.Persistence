using AutoMapper;
using User.Persistence.Application.Events;
using User.Persistence.Application.Interfaces;
using User.Persistence.Domain.Messages.DomaiEvents;
using User.Persistence.Infrastructure.Queue;

namespace User.Persistence.Application.Services;
public class EventsAppService(
    IRabbitMqEventsDispatcher rabbitMqEventsDispatcher,
    IMapper mapper) : IEventAppService
{
    private readonly IRabbitMqEventsDispatcher _rabbitMqEventsDispatcher = rabbitMqEventsDispatcher;
    private readonly IMapper _mapper = mapper;

    public async Task SendUserCreateEvent(UserCreateDomainEvent message)
    {
        await _rabbitMqEventsDispatcher.SendEvent(new OnUserCreateEvent()
        {
            Payload = new OnUserCreateMessage(
                message.Id,
                message.Name,
                message.Email,
                message.Password)
        });
    }
}
