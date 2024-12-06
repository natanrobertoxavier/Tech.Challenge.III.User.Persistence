﻿namespace User.Persistence.Domain.Entities;
public class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
}
