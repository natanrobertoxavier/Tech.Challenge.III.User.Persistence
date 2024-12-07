namespace User.Persistence.Communication.Response;
public class MessageResult(string message)
{
    public string Message { get; set; } = message;
}
