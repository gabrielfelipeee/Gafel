using Gafel.Application.UseCases.Person.Update;
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
    public async Task<IActionResult> Update([FromBody] UpdatePersonCommand request, [FromServices] IUpdatePersonUseCase useCase)
    {
        await useCase.Execute(request);

        return NoContent();
    }
}
