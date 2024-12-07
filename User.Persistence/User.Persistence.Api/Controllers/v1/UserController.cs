using Microsoft.AspNetCore.Mvc;
using User.Persistence.Api.Filters;
using User.Persistence.Application.UseCase.Register;
using User.Persistence.Communication.Request;
using User.Persistence.Communication.Response;

namespace User.Persistence.Api.Controllers.v1;

public class UserController : TechChallengeController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> RegisterUser(
        [FromServices] IRegisterUserUseCase useCase,
        [FromBody] RequestRegisterUserJson request)
    {
        var result = await useCase.RegisterUser(request);

        return Created(string.Empty, result);
    }

    //[HttpPut]
    //[Route("change-password")]
    //[ProducesResponseType(StatusCodes.Status204NoContent)]
    //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
    //[ServiceFilter(typeof(AuthenticatedUserAttribute))]
    //public async Task<IActionResult> ChangePassword(
    //    [FromServices] IChangePasswordUseCase useCase,
    //    [FromBody] RequestChangePasswordJson request)
    //{
    //    await useCase.Execute(request);

    //    return NoContent();
    //}
}
