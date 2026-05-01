using Gafel.Application.UseCases.Category.GetById;
using Gafel.Application.UseCases.Category.Register;
using Gafel.Application.UseCases.Category.Shared.Commands;
using Gafel.Application.UseCases.Category.Shared.Responses;
using Gafel.Application.UseCases.Category.Update;
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

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] long id, [FromBody] CategoryCommand request, [FromServices] IUpdateCategoryUseCase useCase)
    {
        await useCase.Execute(categoryId: id, request: request);

        return NoContent();
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] long id, [FromServices] IGetCategoryByIdUseCase useCase)
    {
        var result = await useCase.Execute(categoryId: id);

        return Ok(result);
    }
}
