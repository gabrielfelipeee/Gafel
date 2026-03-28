using Gafel.Domain.Dtos;
using Gafel.Domain.Services.CurrentUser;
using Moq;

namespace CommonTestUtilities.Services.CurrentUser
{
    public class CurrentUserBuilder
    {
        public static ICurrentUser Build(UserDto user)
        {
            var mock = new Mock<ICurrentUser>();

            mock.Setup(x => x.CurrentUser()).Returns(user);

            return mock.Object;
        }
    }
}
