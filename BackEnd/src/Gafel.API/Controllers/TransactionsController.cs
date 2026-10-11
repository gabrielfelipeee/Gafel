using Gafel.Application.UseCases.Shared.Responses;
using Gafel.Application.UseCases.Transaction.Create;
using Gafel.Application.UseCases.Transaction.Delete;
using Gafel.Application.UseCases.Transaction.Filter;
using Gafel.Application.UseCases.Transaction.GetById;
using Gafel.Application.UseCases.Transaction.Shared.Commands;
using Gafel.Application.UseCases.Transaction.Shared.Responses;
using Gafel.Application.UseCases.Transaction.Update;
using Gafel.Domain.Dtos.QueryParams;
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

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, [FromServices] IGetTransactionByIdUseCase useCase)
    {
        var result = await useCase.Execute(id);

        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginationResponse<TransactionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Filter([FromQuery] FilterTransactionQueryParams queryParams, [FromServices] IFilterTransactionUseCase useCase)
    {
        var result = await useCase.Execute(queryParams);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, [FromServices] IDeleteTransactionUseCase useCase)
    {
        await useCase.Execute(id);

        return NoContent();
    }
}
