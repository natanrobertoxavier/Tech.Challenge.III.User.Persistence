using User.Persistence.Communication.Request;
using User.Persistence.Communication.Response;

namespace User.Persistence.Application.UseCase.Register;
public interface IRegisterUserUseCase
{
    Task<Result<MessageResult>> RegisterUserAsync(RequestRegisterUserJson request);
}
