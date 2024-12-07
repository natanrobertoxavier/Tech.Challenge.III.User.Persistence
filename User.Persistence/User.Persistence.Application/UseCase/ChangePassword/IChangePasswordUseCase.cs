using User.Persistence.Communication.Request;
using User.Persistence.Communication.Response;

namespace User.Persistence.Application.UseCase.ChangePassword;
public interface IChangePasswordUseCase
{
    Task<Result<MessageResult>> ChangePassword(RequestChangePasswordJson requisicao);
}
