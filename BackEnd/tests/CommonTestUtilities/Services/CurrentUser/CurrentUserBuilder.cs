using Gafel.Domain.Services.CurrentUser;
using Moq;

namespace CommonTestUtilities.Services.CurrentUser;

public class CurrentUserBuilder
{
    public static ICurrentUser Build()
    {
        var mock = new Mock<ICurrentUser>();

        mock.Setup(r => r.UserId).Returns(1);

        return mock.Object;
    }
}
