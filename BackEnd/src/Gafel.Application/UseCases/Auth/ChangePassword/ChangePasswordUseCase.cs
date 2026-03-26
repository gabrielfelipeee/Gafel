using Gafel.Application.Exceptions;
using Gafel.Application.Extensions;
using Gafel.Domain.Services.CurrentUser;
using Gafel.Domain.Services.Identity;

namespace Gafel.Application.UseCases.Auth.ChangePassword;

public class ChangePasswordUseCase : IChangePasswordUseCase
{
    private readonly IAuthService _authService;
    private readonly ICurrentUser _currentUser;
    public ChangePasswordUseCase(IAuthService authService, ICurrentUser currentUser)
    {
        _authService = authService;
        _currentUser = currentUser;
    }

    public async Task Execute(ChangePasswordCommand request)
    {
        Validate(request);

        var currentUserId = _currentUser.UserId;

        var result = await _authService.ChangePassword(userId: currentUserId, password: request.Password, newPassword: request.NewPassword);
        if (!result.Success)
            throw new ErrorOnValidationException(result.Errors!);
    }

    private static void Validate(ChangePasswordCommand request)
    {
        var result = new ChangePasswordValidator().Validate(request);

        if (!result.IsValid)
        {
            var errors = result.Errors.ToErrorsByPropertyDictionary();

            throw new ErrorOnValidationException(errors);
        }
    }
}
