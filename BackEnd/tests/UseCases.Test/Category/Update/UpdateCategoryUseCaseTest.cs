using CommonTestUtilities.Commands;
using CommonTestUtilities.Dtos;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Category;
using CommonTestUtilities.Repositories.Person;
using CommonTestUtilities.Services.CurrentUser;
using Gafel.Application.Exceptions;
using Gafel.Application.UseCases.Category.Update;
using Gafel.Domain.Dtos;
using Gafel.Domain.Enums;
using Gafel.Domain.Resources;
using Shouldly;
using System.Net;

namespace UseCases.Test.Category.Update;

public class UpdateCategoryUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        // Arrange 
        var user = UserDtoBuilder.Build();
        var person = PersonBuilder.Build();
        var category = CategoryBuilder.Build(person);
        var request = CategoryCommandBuilder.Build();

        var useCase = CreateUseCase(user: user, person: person, category: category);

        //Act 
        async Task act() => await useCase.Execute(categoryId: category.Id, request: request);

        // Assert
        await Should.NotThrowAsync(act);
    }

    [Fact]
    public async Task Error_Category_NotFound()
    {
        var user = UserDtoBuilder.Build();
        var person = PersonBuilder.Build();
        var request = CategoryCommandBuilder.Build();

        var useCase = CreateUseCase(user: user, person: person);

        var exception = await Should.ThrowAsync<NotFoundException>(useCase.Execute(categoryId: 100, request: request));

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_NOT_FOUND_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.CATEGORY_NOT_FOUND);
        exception.GetStatusCode().ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var user = UserDtoBuilder.Build();
        var person = PersonBuilder.Build();
        var category = CategoryBuilder.Build(person);
        var request = CategoryCommandBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase(user: user, person: person, category: category);

        var exception = await Should.ThrowAsync<ErrorOnValidationException>(useCase.Execute(categoryId: category.Id, request: request));

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
        var category = CategoryBuilder.Build(person);
        var request = CategoryCommandBuilder.Build();
        request.Icon = string.Empty;

        var useCase = CreateUseCase(user: user, person: person, category: category);

        var exception = await Should.ThrowAsync<ErrorOnValidationException>(useCase.Execute(categoryId: category.Id, request: request));

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
        var category = CategoryBuilder.Build(person);
        var request = CategoryCommandBuilder.Build();
        request.Type = (CategoryType)100;

        var useCase = CreateUseCase(user: user, person: person, category: category);

        var exception = await Should.ThrowAsync<ErrorOnValidationException>(useCase.Execute(categoryId: category.Id, request: request));

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_DETAIL);

        var error = exception.GetErrors().ShouldHaveSingleItem();
        error.Key.ShouldBe(nameof(request.Type));

        var value = error.Value.ShouldHaveSingleItem();
        value.ShouldBe(ResourceMessagesException.CATEGORY_TYPE_INVALID);
    }

    [Fact]
    public async Task Error_Person_NotFound()
    {
        var user = UserDtoBuilder.Build();
        var request = CategoryCommandBuilder.Build();
        var useCase = CreateUseCase(user: user);

        var exception = await Should.ThrowAsync<PersonNotFoundException>(useCase.Execute(categoryId: 100, request: request));

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_PERSON_NOT_FOUND_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.EXCEPTION_PERSON_NOT_FOUND_DETAIL);
        exception.GetStatusCode().ShouldBe(HttpStatusCode.NotFound);
    }


    private static UpdateCategoryUseCase CreateUseCase(UserDto user, Gafel.Domain.Entities.Person? person = null, Gafel.Domain.Entities.Category? category = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var currentUser = CurrentUserBuilder.Build(user);

        var personReadOnlyRepository = new PersonReadOnlyRepositoryBuilder();
        if (person is not null)
            personReadOnlyRepository.GetByUserId(person);

        var categoryUpdateOnlyRepository = new CategoryUpdateOnlyRepositoryBuilder();
        if (person is not null && category is not null)
            categoryUpdateOnlyRepository.GetById(person, category);

        return new(
            currentUser,
            categoryUpdateOnlyRepository.Build(),
            personReadOnlyRepository.Build(),
            unitOfWork
        );
    }
}
