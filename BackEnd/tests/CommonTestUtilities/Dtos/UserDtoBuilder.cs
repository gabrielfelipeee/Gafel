using Gafel.Domain.Dtos;

namespace CommonTestUtilities.Dtos;

public class UserDtoBuilder
{
    public static UserDto Build()
    {
        return FakerFactory.Create<UserDto>()
            .CustomInstantiator(faker =>
            {
                var email = faker.Internet.Email();

                return new(
                    Id: Guid.CreateVersion7(),
                    Email: email,
                    UserName: email
                );
            });
    }
}
