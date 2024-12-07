using MediatR;
using User.Persistence.Domain.Messages.DomaiEvents;

namespace User.Persistence.Application.Messages.Handlers;
public class UserEventHandler : INotificationHandler<UserCreateDomainEvent>
{
    public Task Handle(UserCreateDomainEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
