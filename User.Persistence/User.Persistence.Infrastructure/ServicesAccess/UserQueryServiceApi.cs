using Serilog;
using User.Persistence.Domain.ResultServices;
using User.Persistence.Domain.Services;

namespace User.Persistence.Infrastructure.ServicesAccess;
public class UserQueryServiceApi(
    IHttpClientFactory httpClientFactory) : IUserQueryServiceApi
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    //private readonly ILogger _logger = logger;

    public async Task<Result<ThereIsUserResult>> ThereIsUserWithEmailAsync(string email)
    {
        var client = _httpClientFactory.CreateClient("UserQueryApi");
        var response = await client.GetAsync("/api/v1/user/there-is-user/natan%40email.com");

        if (response.IsSuccessStatusCode)
        {
            //return await response.Content.ReadAsStringAsync();
            return null;
        }

        throw new Exception($"API call failed with status code {response.StatusCode}");
    }
}
