using CommonTestUtilities.Commands;
using CommonTestUtilities.Dtos;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Category;
using CommonTestUtilities.Repositories.Person;
using CommonTestUtilities.Services.CurrentUser;
using Gafel.Application.Exceptions;
using Gafel.Application.UseCases.Category.Register;
using Gafel.Domain.Dtos;
using Gafel.Domain.Entities;
using Gafel.Domain.Enums;
using Gafel.Domain.Resources;
using Shouldly;

namespace UseCases.Test.Category.Register;

public class RegisterCategoryUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        // Arrange 
        var user = UserDtoBuilder.Build();
        var person = PersonBuilder.Build();
        var request = CategoryCommandBuilder.Build();

        var useCase = CreateUseCase(user: user, person: person);

        //Act 
        async Task act() => await useCase.Execute(request);

        // Assert
        await Should.NotThrowAsync(act);
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var user = UserDtoBuilder.Build();
        var person = PersonBuilder.Build();
        var request = CategoryCommandBuilder.Build();
        request.Name = string.Empty;
        var useCase = CreateUseCase(user: user, person: person);

        var exception = await Should.ThrowAsync<ErrorOnValidationException>(() => useCase.Execute(request));

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_DETAIL);

        var error = exception.GetErrors().ShouldHaveSingleItem();
        error.Key.ShouldBe(nameof(request.Name));

        var value = error.Value.ShouldHaveSingleItem();
        value.ShouldBe(ResourceMessagesException.CATEGORY_NAME_EMPTY);
    }

    [Fact]
    public async Task Error_Icon_Empty()
    {
        var user = UserDtoBuilder.Build();
        var person = PersonBuilder.Build();
        var request = CategoryCommandBuilder.Build();
        request.Icon = string.Empty;
        var useCase = CreateUseCase(user: user, person: person);

        var exception = await Should.ThrowAsync<ErrorOnValidationException>(() => useCase.Execute(request));

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_DETAIL);

        var error = exception.GetErrors().ShouldHaveSingleItem();
        error.Key.ShouldBe(nameof(request.Icon));

        var value = error.Value.ShouldHaveSingleItem();
        value.ShouldBe(ResourceMessagesException.CATEGORY_ICON_EMPTY);
    }

    [Fact]
    public async Task Error_Type_Invalid()
    {
        var user = UserDtoBuilder.Build();
        var person = PersonBuilder.Build();
        var request = CategoryCommandBuilder.Build();
        request.Type = (CategoryType)100;
        var useCase = CreateUseCase(user: user, person: person);

        var exception = await Should.ThrowAsync<ErrorOnValidationException>(() => useCase.Execute(request));

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_DETAIL);

        var error = exception.GetErrors().ShouldHaveSingleItem();
        error.Key.ShouldBe(nameof(request.Type));

        var value = error.Value.ShouldHaveSingleItem();
        value.ShouldBe(ResourceMessagesException.CATEGORY_TYPE_INVALID);
    }


    private static RegisterCategoryUseCase CreateUseCase(UserDto user, Person person)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var currentUser = CurrentUserBuilder.Build(user);

        var personReadOnlyRepository = new PersonReadOnlyRepositoryBuilder();
        personReadOnlyRepository.GetByUserId(person);

        var categoryWriteOnlyRepository = CategoryWriteOnlyRepositoryBuilder.Build();

        return new(
            currentUser,
            categoryWriteOnlyRepository,
            personReadOnlyRepository.Build(),
            unitOfWork
        );
    }
}
