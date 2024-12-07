namespace User.Persistence.Communication.Response;

public class ResponseRegisteredUserJson(string message)
{
    public string Message { get; set; } = message;
}
