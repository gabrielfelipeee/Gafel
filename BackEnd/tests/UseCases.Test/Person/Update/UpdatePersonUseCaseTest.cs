using CommonTestUtilities.Commands;
using CommonTestUtilities.Dtos;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Person;
using CommonTestUtilities.Services.CurrentUser;
using Gafel.Application.Exceptions;
using Gafel.Application.UseCases.Person.Update;
using Gafel.Domain.Resources;
using Shouldly;

namespace UseCases.Test.Person.Update;

public class UpdatePersonUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        // Arrange 
        var user = UserDtoBuilder.Build();
        var person = PersonBuilder.Build();
        var request = UpdatePersonCommandBuilder.Build();

        var useCase = CreateUseCase(user: user, person: person);

        //Act 
        async Task act() => await useCase.Execute(request);

        // Assert
        await Should.NotThrowAsync(act);
    }

    [Fact]
    public async Task Error_Cpf_Invalid()
    {
        var user = UserDtoBuilder.Build();
        var person = PersonBuilder.Build();
        var request = UpdatePersonCommandBuilder.Build();
        request.Cpf = "00000000000";
        var useCase = CreateUseCase(user: user, person: person);

        var exception = await Should.ThrowAsync<ErrorOnValidationException>(() => useCase.Execute(request));

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_DETAIL);

        var error = exception.GetErrors().ShouldHaveSingleItem();
        error.Key.ShouldBe(nameof(request.Cpf));

        var value = error.Value.ShouldHaveSingleItem();
        value.ShouldBe(ResourceMessagesException.CPF_INVALID);
    }

    [Fact]
    public async Task Error_Cpf_Already_Registered_By_Another_User()
    {
        var user = UserDtoBuilder.Build();
        var person = PersonBuilder.Build();
        var request = UpdatePersonCommandBuilder.Build(withCpf: true);
        var useCase = CreateUseCase(user: user, person: person, existPersonWithCpf: true);

        var exception = await Should.ThrowAsync<ErrorOnValidationException>(() => useCase.Execute(request));

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_DETAIL);

        var error = exception.GetErrors().ShouldHaveSingleItem();
        error.Key.ShouldBe(nameof(request.Cpf));

        var value = error.Value.ShouldHaveSingleItem();
        value.ShouldBe(ResourceMessagesException.CPF_ALREADY_REGISTERED);
    }


    [Fact]
    public async Task Error_Cpf_Cannot_Be_Modified_After_Creation()
    {
        var user = UserDtoBuilder.Build();
        var person = PersonBuilder.Build(withCpf: true);
        var request = UpdatePersonCommandBuilder.Build(withCpf: true);
        var useCase = CreateUseCase(user: user, person: person);

        var exception = await Should.ThrowAsync<ErrorOnValidationException>(() => useCase.Execute(request));

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_DETAIL);

        var error = exception.GetErrors().ShouldHaveSingleItem();
        error.Key.ShouldBe(nameof(request.Cpf));

        var value = error.Value.ShouldHaveSingleItem();
        value.ShouldBe(ResourceMessagesException.CPF_UPDATE_NOT_ALLOWED);
    }


    [Fact]
    public async Task Error_DateOfBirth_Invalid()
    {
        var user = UserDtoBuilder.Build();
        var person = PersonBuilder.Build();
        var request = UpdatePersonCommandBuilder.Build();
        request.DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow);
        var useCase = CreateUseCase(user: user, person: person);

        var exception = await Should.ThrowAsync<ErrorOnValidationException>(() => useCase.Execute(request));

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_DETAIL);

        var error = exception.GetErrors().ShouldHaveSingleItem();
        error.Key.ShouldBe(nameof(request.DateOfBirth));

        var value = error.Value.ShouldHaveSingleItem();
        value.ShouldBe(ResourceMessagesException.PERSON_DATE_OF_BIRTH_INVALID);
    }

    [Fact]
    public async Task Error_DateOfBirth_Future()
    {
        var user = UserDtoBuilder.Build();
        var person = PersonBuilder.Build();
        var request = UpdatePersonCommandBuilder.Build();
        request.DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5));
        var useCase = CreateUseCase(user: user, person: person);

        var exception = await Should.ThrowAsync<ErrorOnValidationException>(() => useCase.Execute(request));

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_DETAIL);

        var error = exception.GetErrors().ShouldHaveSingleItem();
        error.Key.ShouldBe(nameof(request.DateOfBirth));

        var value = error.Value.ShouldHaveSingleItem();
        value.ShouldBe(ResourceMessagesException.PERSON_DATE_OF_BIRTH_FUTURE);
    }

    [Fact]
    public async Task Error_DateOfBirth_Cannot_Be_Modified_After_Creation()
    {
        var user = UserDtoBuilder.Build();
        var person = PersonBuilder.Build(withDateOfBirth: true);
        var request = UpdatePersonCommandBuilder.Build(withDateOfBirth: true);
        request.DateOfBirth = request.DateOfBirth!.Value.AddDays(-1);
        var useCase = CreateUseCase(user: user, person: person);

        var exception = await Should.ThrowAsync<ErrorOnValidationException>(() => useCase.Execute(request));

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_DETAIL);

        var error = exception.GetErrors().ShouldHaveSingleItem();
        error.Key.ShouldBe(nameof(request.DateOfBirth));

        var value = error.Value.ShouldHaveSingleItem();
        value.ShouldBe(ResourceMessagesException.PERSON_DATE_OF_BIRTH_UPDATE_NOT_ALLOWED);
    }

    private static UpdatePersonUseCase CreateUseCase(
        Gafel.Domain.Dtos.UserDto user,
        Gafel.Domain.Entities.Person? person = null,
        bool existPersonWithCpf = false)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var currentUser = CurrentUserBuilder.Build(user);

        var personReadOnly = new PersonReadOnlyRepositoryBuilder();
        if (existPersonWithCpf)
            personReadOnly.ExistPersonWithCpf();

        var personUpdateOnly = new PersonUpdateOnlyRepositoryBuilder();
        if (person is not null)
            personUpdateOnly.GetByUserId(person);

        return new(currentUser, personUpdateOnly.Build(), personReadOnly.Build(), unitOfWork);
    }
}
