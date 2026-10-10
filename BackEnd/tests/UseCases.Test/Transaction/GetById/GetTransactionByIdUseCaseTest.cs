using CommonTestUtilities.Dtos;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.Person;
using CommonTestUtilities.Repositories.Transaction;
using CommonTestUtilities.Services.CurrentUser;
using Gafel.Application.Exceptions;
using Gafel.Application.UseCases.Transaction.GetById;
using Gafel.Domain.Dtos;
using Gafel.Domain.Resources;
using Shouldly;
using System.Net;

namespace UseCases.Test.Transaction.GetById;

public class GetTransactionByIdUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        // Arrange 
        var user = UserDtoBuilder.Build();
        var person = PersonBuilder.Build(user);
        var transaction = TransactionBuilder.Build(person, include: true);

        var useCase = CreateUseCase(user: user, person: person, transaction: transaction);

        //Act 
        async Task act() => await useCase.Execute(transaction.Id);

        // Assert
        await Should.NotThrowAsync(act);
    }

    [Fact]
    public async Task Error_Transaction_NotFound()
    {
        var user = UserDtoBuilder.Build();
        var person = PersonBuilder.Build(user);
        var useCase = CreateUseCase(user: user, person: person);

        var exception = await Should.ThrowAsync<NotFoundException>(useCase.Execute(transactionId: Guid.CreateVersion7()));

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_NOT_FOUND_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.TRANSACTION_NOT_FOUND);
        exception.GetStatusCode().ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Error_Person_NotFound()
    {
        var user = UserDtoBuilder.Build();
        var useCase = CreateUseCase(user: user);

        var exception = await Should.ThrowAsync<PersonNotFoundException>(useCase.Execute(transactionId: Guid.CreateVersion7()));

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_PERSON_NOT_FOUND_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.EXCEPTION_PERSON_NOT_FOUND_DETAIL);
        exception.GetStatusCode().ShouldBe(HttpStatusCode.NotFound);
    }


    private static GetTransactionByIdUseCase CreateUseCase(
        UserDto user,
        Gafel.Domain.Entities.Person? person = null,
        Gafel.Domain.Entities.Transaction? transaction = null)
    {
        var currentUser = CurrentUserBuilder.Build(user);
        var personReadOnlyRepository = new PersonReadOnlyRepositoryBuilder();

        var transactionReadOnlyRepository = new TransactionReadOnlyRepositoryBuilder();

        if (person is not null)
        {
            personReadOnlyRepository.GetByUserId(person);

            if (transaction is not null)
                transactionReadOnlyRepository.GetById(person, transaction);
        }

        return new(
            currentUser,
            personReadOnlyRepository.Build(),
            transactionReadOnlyRepository.Build()
        );
    }
}
