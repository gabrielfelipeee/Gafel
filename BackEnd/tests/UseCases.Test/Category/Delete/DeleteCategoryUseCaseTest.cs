using CommonTestUtilities.Dtos;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.BankAccount;
using CommonTestUtilities.Repositories.Category;
using CommonTestUtilities.Repositories.Person;
using CommonTestUtilities.Services.CurrentUser;
using Gafel.Application.Exceptions;
using Gafel.Application.UseCases.Category.Delete;
using Gafel.Domain.Dtos;
using Gafel.Domain.Resources;
using Shouldly;
using System.Net;

namespace UseCases.Test.Category.Delete;

public class DeleteCategoryUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        // Arrange 
        var user = UserDtoBuilder.Build();
        var person = PersonBuilder.Build();
        var category = CategoryBuilder.Build(person);

        var useCase = CreateUseCase(user: user, category: category, person: person);

        //Act 
        async Task act() => await useCase.Execute(category.Id);

        // Assert
        await Should.NotThrowAsync(act);
    }

    [Fact]
    public async Task Error_Category_NotFound()
    {
        var user = UserDtoBuilder.Build();
        var person = PersonBuilder.Build();
        var useCase = CreateUseCase(user: user, person: person);

        var exception = await Should.ThrowAsync<NotFoundException>(useCase.Execute(categoryId: 100));

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_NOT_FOUND_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.CATEGORY_NOT_FOUND);
        exception.GetStatusCode().ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Error_Person_NotFound()
    {
        var user = UserDtoBuilder.Build();
        var useCase = CreateUseCase(user: user);

        var exception = await Should.ThrowAsync<PersonNotFoundException>(useCase.Execute(categoryId: 1));

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_PERSON_NOT_FOUND_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.EXCEPTION_PERSON_NOT_FOUND_DETAIL);
        exception.GetStatusCode().ShouldBe(HttpStatusCode.NotFound);
    }


    private static DeleteCategoryUseCase CreateUseCase(UserDto user, Gafel.Domain.Entities.Category? category = null, Gafel.Domain.Entities.Person? person = null)
    {
        var currentUser = CurrentUserBuilder.Build(user);

        var personReadOnlyRepository = new PersonReadOnlyRepositoryBuilder();
        if (person is not null)
            personReadOnlyRepository.GetByUserId(person);

        var categoryReadOnlyRepository = new CategoryReadOnlyRepositoryBuilder();
        if (person is not null && category is not null)
            categoryReadOnlyRepository.ExistActiveCategoryWithId(person, category.Id);

        var categoryWriteOnlyRepository = CategoryWriteOnlyRepositoryBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();

        return new(
            currentUser,
            personReadOnlyRepository.Build(),
            categoryReadOnlyRepository.Build(),
            categoryWriteOnlyRepository,
            unitOfWork
        );
    }
}
