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
    public async Task<IActionResult> Register([FromBody] RegisterAuthCommand request, [FromServices] IRegisterAuthUseCase useCase)
    {
        var result = await useCase.Execute(request);

        return Created(string.Empty, result);
    }


}
