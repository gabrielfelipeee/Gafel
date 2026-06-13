using Gafel.Application.UseCases.BankAccount.Create;
using Gafel.Application.UseCases.BankAccount.Shared.Commands;
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
}
