using Gafel.Domain.Dtos;
using Gafel.Domain.Resources;
using Gafel.Domain.Services.Identity;
using Moq;

namespace CommonTestUtilities.Services.Identity;

public class AuthServiceBuilder
{
    private readonly Mock<IAuthService> _repository;
    public AuthServiceBuilder() => _repository = new Mock<IAuthService>();

    public IAuthService Build() => _repository.Object;

    public void Login()
    {
        var response = new LoginResponseDto(Success: true, UserId: 1);

        _repository.Setup(r => r.Login(It.IsAny<UserCredentialsDto>())).ReturnsAsync(response);
    }

    public void LoginWithInvalidCredentials()
    {
        var response = new LoginResponseDto(Success: false, ErrorMessage: ResourceMessagesException.AUTH_INVALID_CREDENTIALS);

        _repository.Setup(r => r.Login(It.IsAny<UserCredentialsDto>())).ReturnsAsync(response);
    }
}
