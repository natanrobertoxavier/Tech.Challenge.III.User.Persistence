using MediatR;
using User.Persistence.Domain.Messages;

namespace User.Persistence.Application.Messages;
public class MessagePublisher(IMediator mediatr) : IMessagePublisher
{
    private readonly IMediator _mediatr = mediatr;

    public async Task PublishDomainEvent<T>(T domainEvent) where T : DomainEvent
        => await _mediatr.Publish(domainEvent);
}
