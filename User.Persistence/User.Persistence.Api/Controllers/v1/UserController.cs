using Microsoft.AspNetCore.Mvc;
using User.Persistence.Api.Filters;
using User.Persistence.Application.UseCase.ChangePassword;
using User.Persistence.Application.UseCase.Register;
using User.Persistence.Communication.Request;
using User.Persistence.Communication.Response;

namespace User.Persistence.Api.Controllers.v1;

public class UserController : TechChallengeController
{
    [HttpPost]
    [ProducesResponseType(typeof(Result<MessageResult>), StatusCodes.Status200OK)]
    public async Task<IActionResult> RegisterUserAsync(
        [FromServices] IRegisterUserUseCase useCase,
        [FromBody] RequestRegisterUserJson request)
    {
        var result = await useCase.RegisterUserAsync(request);

        return Ok(result);
    }

    [HttpPut]
    [Route("change-password")]
    [ProducesResponseType(typeof(Result<MessageResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ServiceFilter(typeof(AuthenticatedUserAttribute))]
    public async Task<IActionResult> ChangePassword(
        [FromServices] IChangePasswordUseCase useCase,
        [FromBody] RequestChangePasswordJson request)
    {
        var result = await useCase.ChangePassword(request);

        return Ok(result);
    }
}
