using RabbitMq.Package.Events;

namespace User.Persistence.Infrastructure.Queue;
public interface IRabbitMqEventsDispatcher
{
    Task SendEvent<T>(IRabbitMqEvent<T> eventToSend);
}
