namespace User.Persistence.Api.Settings;

public class RabbitMqSettings
{
    public string Url { get; set; }
    public string User { get; set; }
    public string SecretKey { get; set; }
    public string RabbitMqConnectionString { get; set; }
    public string ComposedConnectionString => RabbitMqConnectionString ?? $"amqp://{User}:{SecretKey}@{Url}";
}
