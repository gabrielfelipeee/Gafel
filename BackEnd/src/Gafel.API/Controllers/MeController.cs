using Gafel.Application.UseCases.Account.GetProfile;
using Gafel.Application.UseCases.Account.UpdateProfile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gafel.API.Controllers;

[Authorize]
public class MeController : GafelController
{
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromBody] UpdateProfileCommand request, [FromServices] IUpdateProfileUseCase useCase)
    {
        await useCase.Execute(request);

        return NoContent();
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetProfileResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfile([FromServices] IGetProfileUseCase useCase)
    {
        var result = await useCase.Execute();

        return Ok(result);
    }
}
