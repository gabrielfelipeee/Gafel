using CommonTestUtilities.Dtos;
using CommonTestUtilities.Dtos.QueryParams;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.Category;
using CommonTestUtilities.Repositories.Person;
using CommonTestUtilities.Services.CurrentUser;
using Gafel.Application.Exceptions;
using Gafel.Application.UseCases.Category.Filter;
using Gafel.Domain.Dtos;
using Gafel.Domain.Enums;
using Gafel.Domain.Resources;
using Shouldly;
using System.Net;

namespace UseCases.Test.Category.Filter;

public class FilterCategoryUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        // Arrange 
        var user = UserDtoBuilder.Build();
        var person = PersonBuilder.Build();
        var categories = CategoryBuilder.Collection(person);
        var request = FilterCategoryQueryParamsBuilder.Build();

        var useCase = CreateUseCase(user: user, person: person, categories: categories);

        //Act 
        async Task act() => await useCase.Execute(request);

        // Assert
        await Should.NotThrowAsync(act);
    }

    [Fact]
    public async Task Error_Type_Invalid()
    {
        var user = UserDtoBuilder.Build();
        var person = PersonBuilder.Build();
        var categories = CategoryBuilder.Collection(person);
        var request = FilterCategoryQueryParamsBuilder.Build(type: (CategoryType)100);

        var useCase = CreateUseCase(user: user, categories: categories);

        var exception = await Should.ThrowAsync<ErrorOnValidationException>(useCase.Execute(request));

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_DETAIL);
        exception.GetStatusCode().ShouldBe(HttpStatusCode.BadRequest);

        var error = exception.GetErrors().ShouldHaveSingleItem();
        error.Key.ShouldBe(nameof(request.Type));

        var value = error.Value.ShouldHaveSingleItem();
        value.ShouldBe(ResourceMessagesException.CATEGORY_TYPE_INVALID);
    }

    [Fact]
    public async Task Error_Person_NotFound()
    {
        var user = UserDtoBuilder.Build();
        var person = PersonBuilder.Build();
        var categories = CategoryBuilder.Collection(person);
        var request = FilterCategoryQueryParamsBuilder.Build();

        var useCase = CreateUseCase(user: user, categories: categories);

        var exception = await Should.ThrowAsync<PersonNotFoundException>(useCase.Execute(request));

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_PERSON_NOT_FOUND_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.EXCEPTION_PERSON_NOT_FOUND_DETAIL);
        exception.GetStatusCode().ShouldBe(HttpStatusCode.NotFound);
    }

    private static FilterCategoryUseCase CreateUseCase(
        UserDto user,
        Gafel.Domain.Entities.Person? person = null,
        IList<Gafel.Domain.Entities.Category>? categories = null)
    {
        var currentUser = CurrentUserBuilder.Build(user);
        var personReadOnlyRepository = new PersonReadOnlyRepositoryBuilder();
        if (person is not null)
            personReadOnlyRepository.GetByUserId(person);

        var categoryReadOnlyRepository = new CategoryReadOnlyRepositoryBuilder();
        if (person is not null && categories?.Count > 0)
            categoryReadOnlyRepository.Filter(person, categories);

        return new(
            currentUser,
            personReadOnlyRepository.Build(),
            categoryReadOnlyRepository.Build()
        );
    }
}
