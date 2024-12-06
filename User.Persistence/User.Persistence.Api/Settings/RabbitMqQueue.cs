namespace User.Persistence.Api.Settings;

public class RabbitMqQueue
{
    public RabbitMqQueue(
        string exchangeName,
        string routingKeyName,
        string exchangeType = null)
    {
        Exchange = new RabbitMqExchange(exchangeName, exchangeType);
        RoutingKey = routingKeyName;
        Name = "default";
    }

    public RabbitMqQueue(
        string exchangeName,
        string routingKeyName,
        string queueName,
        string exchangeType = null)
    {
        Exchange = new RabbitMqExchange(exchangeName, exchangeType);
        queueName = queueName?.Trim();
        RoutingKey = routingKeyName;
        Name = (string.IsNullOrEmpty(queueName) ? (Exchange.Name.Replace(".exchange", string.Empty) + "." + RoutingKey) : queueName);
    }

    public string RoutingKey { get; set; }
    public string Name { get; set; }
    public RabbitMqExchange Exchange { get; }
}

public class RabbitMqExchange
{
    public RabbitMqExchange(
        string name, 
        string type)
    {
        Name = name;
        Type = type ?? "direct";
    }

    public string Name { get; set; }
    public string Type { get; set; }
}
