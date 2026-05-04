using Moq;
using Gafel.Domain.Resources;
using Gafel.Domain.Dtos;
using Gafel.Domain.Dtos.Responses;
using Gafel.Domain.Services.Identity;

namespace CommonTestUtilities.Services.Identity;

public class UserWriteOnlyServiceBuilder
{
    private readonly Mock<IUserWriteOnlyService> _repository;
    public UserWriteOnlyServiceBuilder() => _repository = new Mock<IUserWriteOnlyService>();

    public IUserWriteOnlyService Build() => _repository.Object;

    public void Register()
    {
        var response = new RegisterUserResponseDto(Success: true, UserId: 1);

        _repository
            .Setup(r => r.Register(It.IsAny<UserCredentialsDto>()))
            .ReturnsAsync(response);
    }

    public void RegisterWithEmailAlreadyInUse()
    {
        var errors = new Dictionary<string, string[]>
        {
            { "Email", new[] { ResourceMessagesException.USER_EMAIL_ALREADY_REGISTERED } }
        };

        var response = new RegisterUserResponseDto(Success: false, Errors: errors);

        _repository
            .Setup(r => r.Register(It.IsAny<UserCredentialsDto>()))
            .ReturnsAsync(response);
    }
}
