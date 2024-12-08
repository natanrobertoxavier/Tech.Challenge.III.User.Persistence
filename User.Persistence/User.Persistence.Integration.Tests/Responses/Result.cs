using Newtonsoft.Json;

namespace User.Persistence.Integration.Tests.Responses;
public class Result<T>
{
    [JsonProperty("data")]
    public T Data { get; set; } = default;

    [JsonProperty("isSuccess")]
    public bool IsSuccess { get; set; } = false;

    [JsonProperty("errors")]
    public List<string> Errors { get; set; } = new List<string>();
}
