using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using TokenService.Manager.Controller;
using User.Persistence.Communication.Response;
using User.Persistence.Exceptions;
using User.Persistence.Exceptions.ExceptionBase;

namespace User.Persistence.Api.Filters;

public class AuthenticatedUserAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
{
    private readonly TokenController _tokenController;
    //private readonly IUserReadOnlyRepository _repository;

    public AuthenticatedUserAttribute(TokenController tokenController)
    {
        _tokenController = tokenController;
        //_repository = repository;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            var token = TokenInRequest(context);
            var emailUsuario = _tokenController.RecoverEmail(token);

            //var usuario = await _repository.RecoverByEmailAsync(emailUsuario) ?? throw new TechChallengeException(string.Empty);
        }
        catch (SecurityTokenExpiredException)
        {
            ExpiredToken(context);
        }
        catch
        {
            UserWithoutPermission(context);
        }
    }

    private static string TokenInRequest(AuthorizationFilterContext context)
    {
        var authorization = context.HttpContext.Request.Headers["Authorization"].ToString();

        if (string.IsNullOrWhiteSpace(authorization))
        {
            throw new TechChallengeException(string.Empty);
        }

        return authorization["Bearer".Length..].Trim();
    }

    private static void ExpiredToken(AuthorizationFilterContext context)
    {
        context.Result = new UnauthorizedObjectResult(new ErrorResponseJson(ErrorsMessages.ExpiredToken));
    }

    private static void UserWithoutPermission(AuthorizationFilterContext context)
    {
        context.Result = new UnauthorizedObjectResult(new ErrorResponseJson(ErrorsMessages.UserWithoutPermission));
    }
}
