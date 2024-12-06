using Newtonsoft.Json;

namespace User.Persistence.Domain.ResultServices;
public class Result<T>
{
    [JsonProperty("data")]
    public T Data { get; set; } = default;

    [JsonProperty("isSuccess")]
    public bool IsSuccess { get; set; } = false;

    [JsonProperty("error")]
    public string Error { get; set; } = string.Empty;
}
