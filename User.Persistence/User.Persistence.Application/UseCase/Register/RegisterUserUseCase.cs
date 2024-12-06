using AutoMapper;
using TokenService.Manager.Controller;
using User.Persistence.Communication.Request;
using User.Persistence.Communication.Response;
using User.Persistence.Domain.Repositories;
using User.Persistence.Domain.Services;
using User.Persistence.Exceptions;
using User.Persistence.Exceptions.ExceptionBase;

namespace User.Persistence.Application.UseCase.Register;
public class RegisterUserUseCase(
    IUserQueryServiceApi userQueryServiceApi,
    //IUserWriteOnlyRepository userWriteOnlyRepository,
    IMapper mapper,
    IWorkUnit workUnit,
    PasswordEncryptor passwordEncryptor,
    TokenController tokenController) : IRegisterUserUseCase
{
    private readonly IUserQueryServiceApi _userQueryServiceApi = userQueryServiceApi;
//private readonly IUserWriteOnlyRepository _userWriteOnlyRepository = userWriteOnlyRepository;
private readonly IMapper _mapper = mapper;
private readonly IWorkUnit _workUnit = workUnit;
private readonly PasswordEncryptor _passwordEncryptor = passwordEncryptor;
private readonly TokenController _tokenController = tokenController;

public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
{
    await Validate(request);

    var entity = _mapper.Map<Domain.Entities.User>(request);
    entity.Password = _passwordEncryptor.Encrypt(request.Password);

    //await _userWriteOnlyRepository.Add(entity);
    await _workUnit.Commit();

    var token = _tokenController.GenerateToken(entity.Email);

    return new ResponseRegisteredUserJson
    {
        Token = token
    };
}

private async Task Validate(RequestRegisterUserJson request)
{
    var registerUserValidator = new RegisterUserValidator();
    var validationResult = registerUserValidator.Validate(request);

    var thereIsUserWithEmail = await _userQueryServiceApi.ThereIsUserWithEmailAsync(request.Email);

    if (thereIsUserWithEmail.IsSuccess && thereIsUserWithEmail.Data.ThereIsUser)
    {
        validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("email", ErrorsMessages.EmailAlreadyRegistered));
    }

    if (!validationResult.IsValid)
    {
        var errorMessages = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
        throw new ValidationErrorsException(errorMessages);
    }
}
}
