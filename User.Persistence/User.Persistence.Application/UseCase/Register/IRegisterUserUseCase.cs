using User.Persistence.Communication.Request;
using User.Persistence.Communication.Response;

namespace User.Persistence.Application.UseCase.Register;
public interface IRegisterUserUseCase
{
    Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request);
}
