using CommonTestUtilities.Dtos;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.Person;
using CommonTestUtilities.Services.CurrentUser;
using Gafel.Application.Exceptions;
using Gafel.Application.UseCases.Account.GetProfile;
using Gafel.Domain.Resources;
using System.Net;
using Shouldly;

namespace UseCases.Test.Account.GetProfile;

public class GetProfileUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        // Arrange
        var user = UserDtoBuilder.Build();
        var person = PersonBuilder.Build(true, true);
        var useCase = CreateUseCase(user: user, person: person);

        // Act
        var result = await useCase.Execute();

        // Assert
        result.FullName.ShouldNotBeNullOrWhiteSpace();
        result.FullName.ShouldBe(person.FullName);

        result.Email.ShouldNotBeNullOrWhiteSpace();
        result.Email.ShouldBe(user.Email);

        result.Cpf.ShouldBe(person.Cpf?.Value);
        result.DateOfBirth.ShouldBe(person.DateOfBirth?.Value);

        result.Uf.ShouldBe(person.Uf.ToString());
        result.City.ShouldBe(person.City);
    }

    [Fact]
    public async Task Error_Person_NotFound()
    {
        var user = UserDtoBuilder.Build();
        var useCase = CreateUseCase(user: user);

        var exception = await Should.ThrowAsync<PersonNotFoundException>(useCase.Execute());

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_PERSON_NOT_FOUND_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.EXCEPTION_PERSON_NOT_FOUND_DETAIL);
        exception.GetStatusCode().ShouldBe(HttpStatusCode.NotFound);
    }


    private static GetProfileUseCase CreateUseCase(Gafel.Domain.Dtos.UserDto user, Gafel.Domain.Entities.Person? person = null)
    {
        var currentUser = CurrentUserBuilder.Build(user);
        var personReadOnly = new PersonReadOnlyRepositoryBuilder();
        if (person is not null)
            personReadOnly.GetByUserId(person);

        return new(currentUser, personReadOnly.Build());
    }
}
