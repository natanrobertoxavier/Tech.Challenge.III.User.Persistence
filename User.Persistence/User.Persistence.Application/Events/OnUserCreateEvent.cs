using RabbitMq.Package.Events;
using User.Persistence.Infrastructure.Queue;

namespace User.Persistence.Application.Events;
public class OnUserCreateEvent : IRabbitMqEvent<OnUserCreateMessage>
{
    public string Exchange => RabbitMqConstants.UserPersistenceExchange;

    public string RoutingKey => RabbitMqConstants.RegisterUserRoutingKey;

    public OnUserCreateMessage Payload { get; set; }
}
