namespace User.Persistence.Domain.Messages.DomaiEvents;
public class UserCreateDomainEvent(
    Guid id,
    string name,
    string email,
    string password) : DomainEvent
{
    public Guid Id { get; set; } = id;
    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    public string Name { get; set; } = name;
    public string Email { get; set; } = email;
    public string Password { get; set; } = password;
}
