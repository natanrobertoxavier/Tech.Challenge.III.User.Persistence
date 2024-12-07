using User.Persistence.Domain.Messages.DomaiEvents;

namespace User.Persistence.Application.Interfaces;
public interface IEventAppService
{
    Task SendUserCreateEvent(UserCreateDomainEvent message);
}
