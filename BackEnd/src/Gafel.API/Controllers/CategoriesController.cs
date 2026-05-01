using Gafel.Application.UseCases.Category.Register;
using Gafel.Application.UseCases.Category.Shared.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gafel.API.Controllers;

[Authorize]
public class CategoriesController : GafelController
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] CategoryCommand request, [FromServices] IRegisterCategoryUseCase useCase)
    {
        await useCase.Execute(request);

        return Created();
    }
}
