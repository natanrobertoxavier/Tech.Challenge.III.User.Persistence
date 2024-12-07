using User.Persistence.Domain.ResultServices;

namespace User.Persistence.Domain.Services;
public interface IUserQueryServiceApi
{
    Task<Result<ThereIsUserResult>> ThereIsUserWithEmailAsync(string email);
    Task<Result<UserResult>> RecoverByEmailAsync(string email);
}
