using Serilog;
using TokenService.Manager.Controller;
using User.Persistence.Application.Services.LoggedUser;
using User.Persistence.Communication.Request;
using User.Persistence.Communication.Response;
using User.Persistence.Domain.Repositories;
using User.Persistence.Domain.Repositories.User;
using User.Persistence.Exceptions;
using User.Persistence.Exceptions.ExceptionBase;

namespace User.Persistence.Application.UseCase.ChangePassword;
public class ChangePasswordUseCase(
    IUserUpdateOnlyRepository repository,
    ILoggedUser loggedUser,
    PasswordEncryptor passwordEncryptor,
    IWorkUnit workUnit,
    ILogger logger) : IChangePasswordUseCase
{
    private readonly IUserUpdateOnlyRepository _repository = repository;
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly PasswordEncryptor _passwordEncryptor = passwordEncryptor;
    private readonly IWorkUnit _workUnit = workUnit;
    private readonly ILogger _logger = logger;

    public async Task<Result<MessageResult>> ChangePassword(RequestChangePasswordJson request)
    {
        var output = new Result<MessageResult>();

        try
        {
            _logger.Information($"Start {nameof(ChangePassword)}.");

            var user = await _loggedUser.RecoverUser();

            Validate(request, user);

            user.Password = _passwordEncryptor.Encrypt(request.NewPassword);

            _repository.Update(user);

            await _workUnit.Commit();

            _logger.Information($"End {nameof(ChangePassword)}.");

            return output.Success(new MessageResult("Senha alterada com sucesso."));
        }
        catch (ValidationErrorsException ex)
        {
            var errorMessage = $"There are validations errors: {string.Concat(string.Join(", ", ex.ErrorMessages), ".")}";

            _logger.Error(ex, errorMessage);

            return output.Failure(ex.ErrorMessages);
        }
        catch (Exception ex)
        {
            var errorMessage = string.Format("There are an error: {0}", ex.Message);

            _logger.Error(ex, errorMessage);

            return output.Failure(new List<string>() { errorMessage });
        }
    }

    private void Validate(RequestChangePasswordJson request, Domain.Entities.User user)
    {
        var validator = new ChangePasswordValidator();
        var result = validator.Validate(request);

        var currentPasswordEncrypted = _passwordEncryptor.Encrypt(request.CurrentPassword);

        if (!user.Password.Equals(currentPasswordEncrypted))
        {
            result.Errors.Add(new FluentValidation.Results.ValidationFailure("currentPassword", ErrorsMessages.InvalidCurrentPassword));
        }

        if (!result.IsValid)
        {
            var mensagens = result.Errors.Select(x => x.ErrorMessage).ToList();
            throw new ValidationErrorsException(mensagens);
        }
    }
}
