using FluentValidation;
using User.Persistence.Exceptions;

namespace User.Persistence.Application.UseCase;
public class PasswordValidator : AbstractValidator<string>
{
    public PasswordValidator()
    {
        RuleFor(password => password).NotEmpty().WithMessage(ErrorsMessages.BlankUserPassword);
        When(password => !string.IsNullOrWhiteSpace(password), () =>
        {
            RuleFor(password => password.Length).GreaterThanOrEqualTo(6).WithMessage(ErrorsMessages.MinimumSixCharacters);
        });
    }
}
