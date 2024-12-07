using Serilog;
using MediatR;
using TokenService.Manager.Controller;
using User.Persistence.Application.Events;
using User.Persistence.Communication.Request;
using User.Persistence.Communication.Response;
using User.Persistence.Domain.Messages.DomaiEvents;
using User.Persistence.Domain.Repositories;
using User.Persistence.Domain.Services;
using User.Persistence.Exceptions;
using User.Persistence.Exceptions.ExceptionBase;

namespace User.Persistence.Application.UseCase.Register;
public class RegisterUserUseCase(
    IUserQueryServiceApi userQueryServiceApi,
    IMediator mediator,
    PasswordEncryptor passwordEncryptor,
    ILogger logger) : IRegisterUserUseCase
{
    private readonly IUserQueryServiceApi _userQueryServiceApi = userQueryServiceApi;
    private readonly IMediator _mediator = mediator;
    private readonly PasswordEncryptor _passwordEncryptor = passwordEncryptor;
    private readonly ILogger _logger = logger;

    public async Task<Result<ResponseRegisteredUserJson>> RegisterUser(RequestRegisterUserJson request)
    {
        var output = new Result<ResponseRegisteredUserJson>();

        try
        {
            _logger.Information($"Start {nameof(RegisterUser)}. User: {request.Name}.");

            //await Validate(request);

            var encryptedPassword = _passwordEncryptor.Encrypt(request.Password);

            await _mediator.Publish(new UserCreateDomainEvent(
                Guid.NewGuid(),
                request.Name,
                request.Email,
                encryptedPassword)
            );

            _logger.Information($"End {nameof(RegisterUser)}. User: {request.Name}.");

            return output.Success(new ResponseRegisteredUserJson("Cadastro em processamento."));
        }
        catch (Exception ex)
        {
            var errorMessage = string.Format("There are an error: {0}", ex.Message);

            _logger.Error(ex, errorMessage);

            return output.Failure(errorMessage);
        }
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
