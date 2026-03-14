using Gafel.Application.SharedResponses;
using Gafel.Application.UseCases.Auth.Login.DoLogin;
using Gafel.Application.UseCases.Auth.Register;
using Microsoft.AspNetCore.Mvc;

namespace Gafel.API.Controllers;

public class AuthController : GafelController
{
    [HttpPost("register")]
    [ProducesResponseType(typeof(RegisteredUserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterAccountCommand request, [FromServices] IRegisterAccountUseCase useCase)
    {
        var result = await useCase.Execute(request);

        return Created(string.Empty, result);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(RegisteredUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] DoLoginCommand request, [FromServices] IDoLoginUseCase useCase)
    {
        var result = await useCase.Execute(request);

        return Ok(result);
    }
}
