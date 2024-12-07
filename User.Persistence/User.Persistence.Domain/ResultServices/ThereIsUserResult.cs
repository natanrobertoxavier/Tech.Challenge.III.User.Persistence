using Newtonsoft.Json;

namespace User.Persistence.Domain.ResultServices;
public class ThereIsUserResult
{
    [JsonProperty("thereIsUser")]
    public bool ThereIsUser { get; set; }
}
