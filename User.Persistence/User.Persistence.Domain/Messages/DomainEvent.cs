using MediatR;

namespace User.Persistence.Domain.Messages;
public abstract class DomainEvent : BaseMessage, INotification
{
    protected DomainEvent() { }
}
