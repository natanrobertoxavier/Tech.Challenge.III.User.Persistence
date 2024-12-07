﻿namespace User.Persistence.Infrastructure.Queue;
public static class RabbitMqConstants
{
    public const string UserPersistenceExchange = $"user.persistence.exchange";

    public const string RegisterUserQueueName = "user.persistence.on-register-user";
    public const string RegisterUserRoutingKey = "on-register-user";

    public const string ChangePasswordUserQueueName = "user.persistence.on-change-password";
    public const string ChangePasswordUserRoutingKey = "on-change-password";
}
