using CommonTestUtilities.Dtos;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.BankAccount;
using CommonTestUtilities.Repositories.Person;
using CommonTestUtilities.Services.CurrentUser;
using Gafel.Application.Exceptions;
using Gafel.Application.UseCases.BankAccount.Delete;
using Gafel.Domain.Dtos;
using Gafel.Domain.Resources;
using Shouldly;
using System.Net;

namespace UseCases.Test.BankAccount.Delete;

public class DeleteBankAccountUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        // Arrange 
        var user = UserDtoBuilder.Build();
        var person = PersonBuilder.Build();
        var bankAccount = BankAccountBuilder.Build(person);

        var useCase = CreateUseCase(user: user, person: person, bankAccount: bankAccount);

        //Act 
        async Task act() => await useCase.Execute(bankAccount.Id);

        // Assert
        await Should.NotThrowAsync(act);
    }

    [Fact]
    public async Task Error_BankAccount_NotFound()
    {
        var user = UserDtoBuilder.Build();
        var person = PersonBuilder.Build();
        var useCase = CreateUseCase(user: user, person: person);

        var exception = await Should.ThrowAsync<NotFoundException>(useCase.Execute(bankAccountId: 100));

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_NOT_FOUND_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.BANK_ACCOUNT_NOT_FOUND);
        exception.GetStatusCode().ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Error_Person_NotFound()
    {
        var user = UserDtoBuilder.Build();
        var useCase = CreateUseCase(user: user);

        var exception = await Should.ThrowAsync<PersonNotFoundException>(useCase.Execute(bankAccountId: 1));

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_PERSON_NOT_FOUND_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.EXCEPTION_PERSON_NOT_FOUND_DETAIL);
        exception.GetStatusCode().ShouldBe(HttpStatusCode.NotFound);
    }


    private static DeleteBankAccountUseCase CreateUseCase(UserDto user, Gafel.Domain.Entities.Person? person = null, Gafel.Domain.Entities.BankAccount? bankAccount = null)
    {
        var currentUser = CurrentUserBuilder.Build(user);

        var personReadOnlyRepository = new PersonReadOnlyRepositoryBuilder();
        if (person is not null)
            personReadOnlyRepository.GetByUserId(person);

        var bankAccountReadOnlyRepository = new BankAccountReadOnlyRepositoryBuilder();
        if (person is not null && bankAccount is not null)
            bankAccountReadOnlyRepository.ExistActiveBankAccountWithId(person, bankAccount.Id);

        var bankAccountWriteOnlyRepository = BankAccountWriteOnlyRepositoryBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();

        return new(
            currentUser,
            personReadOnlyRepository.Build(),
            bankAccountReadOnlyRepository.Build(),
            bankAccountWriteOnlyRepository,
            unitOfWork
        );
    }
}
