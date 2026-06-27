using CommonTestUtilities.Commands;
using CommonTestUtilities.Dtos;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.BankAccount;
using CommonTestUtilities.Repositories.Person;
using CommonTestUtilities.Services.CurrentUser;
using Gafel.Application.Exceptions;
using Gafel.Application.UseCases.BankAccount.Update;
using Gafel.Domain.Dtos;
using Gafel.Domain.Enums;
using Gafel.Domain.Resources;
using Shouldly;
using System.Net;

namespace UseCases.Test.BankAccount.Update;

public class UpdateBankAccountUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        // Arrange 
        var user = UserDtoBuilder.Build();
        var person = PersonBuilder.Build();
        var bankAccount = BankAccountBuilder.Build(person);
        var request = BankAccountCommandBuilder.Build();

        var useCase = CreateUseCase(user: user, person: person, bankAccount: bankAccount);

        //Act 
        async Task act() => await useCase.Execute(bankAccountId: bankAccount.Id, request: request);

        // Assert
        await Should.NotThrowAsync(act);
    }

    [Fact]
    public async Task Error_BankAccount_NotFound()
    {
        var user = UserDtoBuilder.Build();
        var person = PersonBuilder.Build();
        var request = BankAccountCommandBuilder.Build();

        var useCase = CreateUseCase(user: user, person: person);

        var exception = await Should.ThrowAsync<NotFoundException>(useCase.Execute(bankAccountId: 100, request: request));

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_NOT_FOUND_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.BANK_ACCOUNT_NOT_FOUND);
        exception.GetStatusCode().ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var user = UserDtoBuilder.Build();
        var person = PersonBuilder.Build();
        var bankAccount = BankAccountBuilder.Build(person);
        var request = BankAccountCommandBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase(user: user, person: person, bankAccount: bankAccount);

        var exception = await Should.ThrowAsync<ErrorOnValidationException>(useCase.Execute(bankAccountId: bankAccount.Id, request: request));

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_DETAIL);

        var error = exception.GetErrors().ShouldHaveSingleItem();
        error.Key.ShouldBe(nameof(request.Name));

        var value = error.Value.ShouldHaveSingleItem();
        value.ShouldBe(ResourceMessagesException.BANK_ACCOUNT_NAME_EMPTY);
    }

    [Fact]
    public async Task Error_InitialBalance_Negative()
    {
        var user = UserDtoBuilder.Build();
        var person = PersonBuilder.Build();
        var bankAccount = BankAccountBuilder.Build(person);
        var request = BankAccountCommandBuilder.Build();
        request.InitialBalance = -100;

        var useCase = CreateUseCase(user: user, person: person, bankAccount: bankAccount);

        var exception = await Should.ThrowAsync<ErrorOnValidationException>(useCase.Execute(bankAccountId: bankAccount.Id, request: request));

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_DETAIL);

        var error = exception.GetErrors().ShouldHaveSingleItem();
        error.Key.ShouldBe(nameof(request.InitialBalance));

        var value = error.Value.ShouldHaveSingleItem();
        value.ShouldBe(ResourceMessagesException.INVALID_INITIAL_BALANCE);
    }

    [Fact]
    public async Task Error_Type_Invalid()
    {
        var user = UserDtoBuilder.Build();
        var person = PersonBuilder.Build();
        var bankAccount = BankAccountBuilder.Build(person);
        var request = BankAccountCommandBuilder.Build();
        request.Type = (BankAccountType)100;

        var useCase = CreateUseCase(user: user, person: person, bankAccount: bankAccount);

        var exception = await Should.ThrowAsync<ErrorOnValidationException>(useCase.Execute(bankAccountId: bankAccount.Id, request: request));

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_DETAIL);

        var error = exception.GetErrors().ShouldHaveSingleItem();
        error.Key.ShouldBe(nameof(request.Type));

        var value = error.Value.ShouldHaveSingleItem();
        value.ShouldBe(ResourceMessagesException.BANK_ACCOUNT_TYPE_INVALID);
    }

    [Fact]
    public async Task Error_Person_NotFound()
    {
        var user = UserDtoBuilder.Build();
        var request = BankAccountCommandBuilder.Build();
        var useCase = CreateUseCase(user: user);

        var exception = await Should.ThrowAsync<PersonNotFoundException>(useCase.Execute(bankAccountId: 100, request: request));

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_PERSON_NOT_FOUND_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.EXCEPTION_PERSON_NOT_FOUND_DETAIL);
        exception.GetStatusCode().ShouldBe(HttpStatusCode.NotFound);
    }


    private static UpdateBankAccountUseCase CreateUseCase(UserDto user, Gafel.Domain.Entities.Person? person = null, Gafel.Domain.Entities.BankAccount? bankAccount = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var currentUser = CurrentUserBuilder.Build(user);

        var personReadOnlyRepository = new PersonReadOnlyRepositoryBuilder();
        if (person is not null)
            personReadOnlyRepository.GetByUserId(person);

        var bankAccountUpdateOnlyRepository = new BankAccountUpdateOnlyRepositoryBuilder();
        if (person is not null && bankAccount is not null)
            bankAccountUpdateOnlyRepository.GetById(person, bankAccount);

        return new(
            currentUser,
            bankAccountUpdateOnlyRepository.Build(),
            personReadOnlyRepository.Build(),
            unitOfWork
        );
    }
}
