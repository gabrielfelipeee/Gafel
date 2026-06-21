using Gafel.API.Binders;
using Gafel.API.Models;
using Gafel.Application.UseCases.BankAccount.Create;
using Gafel.Application.UseCases.BankAccount.Delete;
using Gafel.Application.UseCases.BankAccount.GetById;
using Gafel.Application.UseCases.BankAccount.Shared.Commands;
using Gafel.Application.UseCases.BankAccount.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gafel.API.Controllers;

[Authorize]
public class BankAccountsController : GafelController
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] BankAccountCommand request, [FromServices] ICreateBankAccountUseCase useCase)
    {
        await useCase.Execute(request);

        return Created();
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BankAccountResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute][ModelBinder(typeof(GafelIdBinder))] GafelId id, [FromServices] IGetBankAccountByIdUseCase useCase)
    {
        var result = await useCase.Execute(bankAccountId: id.Value);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute][ModelBinder(typeof(GafelIdBinder))] GafelId id, [FromServices] IDeleteBankAccountUseCase useCase)
    {
        await useCase.Execute(id.Value);

        return NoContent();
    }
}
