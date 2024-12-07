namespace User.Persistence.Domain.Messages;
public interface IMessagePublisher
{
    Task PublishDomainEvent<T>(T domainEvent) where T : DomainEvent;
}
