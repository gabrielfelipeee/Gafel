using Gafel.Domain.Dtos;
using Bogus;

namespace CommonTestUtilities.Dtos;

public class UserDtoBuilder
{
    public static UserDto Build()
    {
        return new Faker<UserDto>("pt_BR")
            .CustomInstantiator(faker =>
            {
                var email = faker.Internet.Email();

                return new(
                    Id: 1,
                    Email: email,
                    UserName: email
                );
            });
    }
}
