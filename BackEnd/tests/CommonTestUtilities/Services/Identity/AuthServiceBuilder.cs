using Gafel.Domain.Dtos;
using Gafel.Domain.Resources;
using Gafel.Domain.Services.Identity;
using Gafel.Domain.Dtos.Responses;
using Moq;

namespace CommonTestUtilities.Services.Identity;

public class AuthServiceBuilder
{
    private readonly Mock<IAuthService> _repository;
    public AuthServiceBuilder() => _repository = new Mock<IAuthService>();

    public IAuthService Build() => _repository.Object;

    public void Login(UserDto user)
    {
        var response = new LoginResponseDto(Success: true, UserId: user.Id);

        _repository.Setup(r => r.Login(It.IsAny<UserCredentialsDto>())).ReturnsAsync(response);
    }
    public void LoginWithInvalidCredentials()
    {
        var response = new LoginResponseDto(Success: false, ErrorMessage: ResourceMessagesException.AUTH_CREDENTIALS_INVALID);

        _repository.Setup(r => r.Login(It.IsAny<UserCredentialsDto>())).ReturnsAsync(response);
    }

    public void ChangePassword()
    {
        var response = new ChangePasswordResponseDto(Success: true);

        _repository.Setup(r => r.ChangePassword(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(response);
    }
    public void ChangePasswordWithCurrentPasswordDifferent()
    {
        var errors = new Dictionary<string, string[]>
        {
            { "Password", new[] { ResourceMessagesException.USER_CURRENT_PASSWORD_INCORRECT } }
        };
        var response = new ChangePasswordResponseDto(Success: false, Errors: errors);

        _repository.Setup(r => r.ChangePassword(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(response);
    }
}
