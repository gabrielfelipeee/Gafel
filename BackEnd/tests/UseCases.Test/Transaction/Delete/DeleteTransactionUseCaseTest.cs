using CommonTestUtilities.Dtos;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Person;
using CommonTestUtilities.Repositories.Transaction;
using CommonTestUtilities.Services.CurrentUser;
using Gafel.Application.Exceptions;
using Gafel.Application.UseCases.Transaction.Delete;
using Gafel.Domain.Dtos;
using Gafel.Domain.Entities;
using Gafel.Domain.Resources;
using Shouldly;
using System.Net;

namespace UseCases.Test.Transaction.Delete;

public class DeleteTransactionUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        // Arrange 
        var user = UserDtoBuilder.Build();
        var person = PersonBuilder.Build(user);
        var transaction = TransactionBuilder.Build(person);

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

    private static DeleteTransactionUseCase CreateUseCase(
        UserDto user,
        Person? person = null,
        Gafel.Domain.Entities.Transaction? transaction = null)
    {
        var currentUser = CurrentUserBuilder.Build(user);
        var unitOfWork = UnitOfWorkBuilder.Build();

        var transactionWriteOnlyRepository = TransactionWriteOnlyRepositoryBuilder.Build();
        var transactionReadOnlyRepository = new TransactionReadOnlyRepositoryBuilder();

        var personReadOnlyRepository = new PersonReadOnlyRepositoryBuilder();
        if (person is not null)
        {
            personReadOnlyRepository.GetByUserId(person);

            if (transaction is not null)
                transactionReadOnlyRepository.ExistActiveTransactionWithId(person, transaction.Id);
        }

        return new(
            currentUser,
            personReadOnlyRepository.Build(),
            transactionReadOnlyRepository.Build(),
            transactionWriteOnlyRepository,
            unitOfWork
        );
    }
}
