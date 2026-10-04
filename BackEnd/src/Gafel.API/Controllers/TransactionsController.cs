using Gafel.Application.UseCases.Transaction.Create;
using Gafel.Application.UseCases.Transaction.Shared.Commands;
using Gafel.Application.UseCases.Transaction.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gafel.API.Controllers;

[Authorize]
public class TransactionsController : GafelController
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] TransactionCommand request, [FromServices] ICreateTransactionUseCase useCase)
    {
        await useCase.Execute(request);

        return Created();
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] TransactionCommand request, [FromServices] IUpdateTransactionUseCase useCase)
    {
        await useCase.Execute(transactionId: id, request: request);

        return NoContent();
    }
}
