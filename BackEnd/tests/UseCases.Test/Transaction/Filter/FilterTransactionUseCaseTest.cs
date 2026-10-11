using CommonTestUtilities.Dtos;
using CommonTestUtilities.Dtos.QueryParams;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.Person;
using CommonTestUtilities.Repositories.Transaction;
using CommonTestUtilities.Services.CurrentUser;
using Gafel.Application.Exceptions;
using Gafel.Application.UseCases.Transaction.Filter;
using Gafel.Domain.Dtos;
using Gafel.Domain.Enums;
using Gafel.Domain.Resources;
using Microsoft.Extensions.Time.Testing;
using Shouldly;
using System.Net;

namespace UseCases.Test.Transaction.Filter;

public class FilterTransactionUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        // Arrange 
        var user = UserDtoBuilder.Build();
        var person = PersonBuilder.Build(user);
        var transactions = TransactionBuilder.Collection(person);
        var request = FilterTransactionQueryParamsBuilder.Build();

        var useCase = CreateUseCase(user: user, person: person, transactions: transactions);

        //Act 
        async Task act() => await useCase.Execute(request);

        // Assert
        await Should.NotThrowAsync(act);
    }

    [Fact]
    public async Task Error_Type_Invalid()
    {
        var user = UserDtoBuilder.Build();
        var person = PersonBuilder.Build(user);
        var transactions = TransactionBuilder.Collection(person);
        var request = FilterTransactionQueryParamsBuilder.Build();
        request.Type = (CategoryType)100;

        var useCase = CreateUseCase(user: user, person: person, transactions: transactions);

        var exception = await Should.ThrowAsync<ErrorOnValidationException>(useCase.Execute(request));

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_DETAIL);
        exception.GetStatusCode().ShouldBe(HttpStatusCode.BadRequest);

        var error = exception.GetErrors().ShouldHaveSingleItem();
        error.Key.ShouldBe(nameof(request.Type));

        var value = error.Value.ShouldHaveSingleItem();
        value.ShouldBe(ResourceMessagesException.TYPE_INVALID);
    }

    [Fact]
    public async Task Error_Person_NotFound()
    {
        var user = UserDtoBuilder.Build();
        var person = PersonBuilder.Build(user);
        var transactions = TransactionBuilder.Collection(person);
        var request = FilterTransactionQueryParamsBuilder.Build();

        var useCase = CreateUseCase(user: user, transactions: transactions);

        var exception = await Should.ThrowAsync<PersonNotFoundException>(useCase.Execute(request));

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_PERSON_NOT_FOUND_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.EXCEPTION_PERSON_NOT_FOUND_DETAIL);
        exception.GetStatusCode().ShouldBe(HttpStatusCode.NotFound);
    }

    private static FilterTransactionUseCase CreateUseCase(
        UserDto user,
        Gafel.Domain.Entities.Person? person = null,
        IList<Gafel.Domain.Entities.Transaction>? transactions = null)
    {
        var currentUser = CurrentUserBuilder.Build(user);
        var personReadOnlyRepository = new PersonReadOnlyRepositoryBuilder();

        var fakeTimeProvider = new FakeTimeProvider(new DateTimeOffset(DateTimeOffset.UtcNow.Date, TimeSpan.Zero));

        var transactionReadOnlyRepository = new TransactionReadOnlyRepositoryBuilder();

        if (person is not null)
        {
            personReadOnlyRepository.GetByUserId(person);

            if (transactions?.Count > 0)
                transactionReadOnlyRepository.Filter(person, transactions);
        }

        return new(
            fakeTimeProvider,
            currentUser,
            personReadOnlyRepository.Build(),
            transactionReadOnlyRepository.Build()
        );
    }
}
