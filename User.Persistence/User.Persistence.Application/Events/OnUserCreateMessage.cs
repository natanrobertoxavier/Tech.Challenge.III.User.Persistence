namespace User.Persistence.Application.Events;
public class OnUserCreateMessage(
    Guid id,
    string name,
    string email,
    string password)
{
    public Guid Id { get; set; } = id;
    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    public string Name { get; set; } = name;
    public string Email { get; set; } = email;
    public string Password { get; set; } = password;
}
