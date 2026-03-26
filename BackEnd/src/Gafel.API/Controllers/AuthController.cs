using Gafel.Application.UseCases.Auth.ChangePassword;
using Gafel.Application.UseCases.Auth.Login.DoLogin;
using Gafel.Application.UseCases.Auth.Register;
using Gafel.Application.UseCases.Auth.SharedResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gafel.API.Controllers;

public class AuthController : GafelController
{
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterAccountCommand request, [FromServices] IRegisterAccountUseCase useCase)
    {
        var result = await useCase.Execute(request);

        return Created(string.Empty, result);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] DoLoginCommand request, [FromServices] IDoLoginUseCase useCase)
    {
        var result = await useCase.Execute(request);

        return Ok(result);
    }

    [Authorize]
    [HttpPut("change-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangePassword([FromServices] IChangePasswordUseCase useCase, [FromBody] ChangePasswordCommand request)
    {
        await useCase.Execute(request);

        return NoContent();
    }
}
