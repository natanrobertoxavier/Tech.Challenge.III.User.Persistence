using FluentValidation;
using User.Persistence.Communication.Request;

namespace User.Persistence.Application.UseCase.ChangePassword;
public class ChangePasswordValidator : AbstractValidator<RequestChangePasswordJson>
{
    public ChangePasswordValidator()
    {
        RuleFor(c => c.NewPassword).SetValidator(new PasswordValidator());
    }
}