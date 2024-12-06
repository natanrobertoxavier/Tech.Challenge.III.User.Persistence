using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;
using System.Reflection;
using User.Persistence.Domain.Services;

namespace User.Persistence.Api.Settings;

public static class Extensions
{
    private static string assemblyName = Assembly.GetEntryAssembly().GetName().Name;

    public static RabbitMQ.Client.IConnectionFactory AddQueueHandler(this IServiceCollection services, string connectionString, ServiceLifetime lifetime = ServiceLifetime.Singleton)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentNullException("connectionString", "RabbitMq connection string not provided.");
        }

        if (lifetime == ServiceLifetime.Scoped)
        {
            throw new ArgumentNullException("lifetime", "Lifetime cannot be scoped.");
        }

        ConnectionFactory connectionFactory = new ConnectionFactory
        {
            Uri = new Uri(connectionString)
        };

        services.Add(ServiceDescriptor.Describe(typeof(RabbitMQ.Client.IConnectionFactory), (IServiceProvider e) => connectionFactory, lifetime));
        services.Add(ServiceDescriptor.Describe(typeof(IRabbitMqQueueHandler), (IServiceProvider e) => connectionFactory, lifetime));

        return connectionFactory;
    }

    public static RabbitMQ.Client.IConnectionFactory DeclareQueues(this RabbitMQ.Client.IConnectionFactory connectionFactory, params RabbitMqQueue[] queues)
    {
        DeclareQueues(connectionFactory.CreateConnection(assemblyName), queues);
        return connectionFactory;
    }

    private static void DeclareQueues(RabbitMQ.Client.IConnection connection, RabbitMqQueue[] queues)
    {
        using RabbitMQ.Client.IModel model = connection.CreateModel();
        foreach (RabbitMqQueue rabbitMqQueue in queues)
        {
            DeclareExchange(model, rabbitMqQueue.Exchange);
            DeclareQueue(model, rabbitMqQueue);
        }
    }

    private static void DeclareExchange(RabbitMQ.Client.IModel model, RabbitMqExchange exchange)
    {
        model.ExchangeDeclare(exchange.Name, exchange.Type, durable: true);
        string deadLetterExchangeName = GetDeadLetterExchangeName(exchange);
        model.ExchangeDeclare(deadLetterExchangeName, exchange.Type, durable: true);
    }

    private static void DeclareQueue(RabbitMQ.Client.IModel model, RabbitMqQueue queue)
    {
        model.QueueDeclare(queue.Name, durable: true, exclusive: false, autoDelete: false);
        model.QueueBind(queue.Name, queue.Exchange.Name, queue.RoutingKey);
        DeclareDeadLetterQueue(model, queue);
    }

    private static void DeclareDeadLetterQueue(RabbitMQ.Client.IModel model, RabbitMqQueue queue)
    {
        string deadLetterQueueName = GetDeadLetterQueueName(queue);
        string deadLetterExchangeName = GetDeadLetterExchangeName(queue.Exchange);

        model.QueueDeclare(deadLetterQueueName, durable: true, exclusive: false, autoDelete: false);
        model.QueueBind(deadLetterQueueName, deadLetterExchangeName, queue.RoutingKey);
    }

    private static string GetDeadLetterExchangeName(RabbitMqExchange exchange) =>
        string.Concat(exchange.Name, ".dead");

    private static string GetDeadLetterQueueName(RabbitMqQueue queue) =>
        string.Concat(queue.Name, "-dlx");
}
