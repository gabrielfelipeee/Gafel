using CommonTestUtilities.Commands;
using CommonTestUtilities.Dtos;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.BankAccount;
using CommonTestUtilities.Repositories.Category;
using CommonTestUtilities.Repositories.Person;
using CommonTestUtilities.Repositories.Transaction;
using CommonTestUtilities.Services.CurrentUser;
using Gafel.Application.Exceptions;
using Gafel.Application.UseCases.Transaction.Update;
using Gafel.Domain.Constants;
using Gafel.Domain.Dtos;
using Gafel.Domain.Resources;
using Shouldly;
using System.Net;

namespace UseCases.Test.Transaction.Update;

public class UpdateTransactionUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        // Arrange 
        var user = UserDtoBuilder.Build();
        var person = PersonBuilder.Build(user);
        var transaction = TransactionBuilder.Build(person);
        var request = TransactionCommandBuilder.Build();

        var useCase = CreateUseCase(user, person, transaction, bankAccountId: request.BankAccountId, categoryId: request.CategoryId);

        //Act 
        async Task act() => await useCase.Execute(transactionId: transaction.Id, request: request);

        // Assert
        await Should.NotThrowAsync(act);
    }

    [Fact]
    public async Task Error_Transaction_NotFound()
    {
        var user = UserDtoBuilder.Build();
        var person = PersonBuilder.Build(user);
        var request = TransactionCommandBuilder.Build();

        var useCase = CreateUseCase(user: user, person: person, bankAccountId: request.BankAccountId, categoryId: request.CategoryId);

        var exception = await Should.ThrowAsync<NotFoundException>(useCase.Execute(transactionId: Guid.CreateVersion7(), request: request));

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_NOT_FOUND_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.TRANSACTION_NOT_FOUND);
        exception.GetStatusCode().ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Error_BankAccountId_Empty()
    {
        var user = UserDtoBuilder.Build();
        var person = PersonBuilder.Build(user);
        var transaction = TransactionBuilder.Build(person);
        var request = TransactionCommandBuilder.Build(bankAccountId: Guid.Empty);

        var useCase = CreateUseCase(user, person, transaction, bankAccountId: request.BankAccountId, categoryId: request.CategoryId);

        var exception = await Should.ThrowAsync<ErrorOnValidationException>(() => useCase.Execute(transactionId: transaction.Id, request));

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_DETAIL);

        var error = exception.GetErrors().ShouldHaveSingleItem();
        error.Key.ShouldBe(nameof(request.BankAccountId));

        var value = error.Value.ShouldHaveSingleItem();
        value.ShouldBe(ResourceMessagesException.TRANSACTION_BANK_ACCOUNT_REQUIRED);
    }

    [Fact]
    public async Task Error_BankAccount_NotFound()
    {
        var user = UserDtoBuilder.Build();
        var person = PersonBuilder.Build(user);
        var transaction = TransactionBuilder.Build(person);
        var request = TransactionCommandBuilder.Build();

        var useCase = CreateUseCase(user, person, transaction, categoryId: request.CategoryId);

        var exception = await Should.ThrowAsync<NotFoundException>(() => useCase.Execute(transactionId: transaction.Id, request));

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_NOT_FOUND_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.BANK_ACCOUNT_NOT_FOUND);
        exception.GetStatusCode().ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Error_CategoryId_Empty()
    {
        var user = UserDtoBuilder.Build();
        var person = PersonBuilder.Build(user);
        var transaction = TransactionBuilder.Build(person);
        var request = TransactionCommandBuilder.Build(categoryId: Guid.Empty);
        var useCase = CreateUseCase(user, person, bankAccountId: request.BankAccountId, categoryId: request.CategoryId);

        var exception = await Should.ThrowAsync<ErrorOnValidationException>(() => useCase.Execute(transactionId: transaction.Id, request));

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_DETAIL);

        var error = exception.GetErrors().ShouldHaveSingleItem();
        error.Key.ShouldBe(nameof(request.CategoryId));

        var value = error.Value.ShouldHaveSingleItem();
        value.ShouldBe(ResourceMessagesException.TRANSACTION_CATEGORY_REQUIRED);
    }

    [Fact]
    public async Task Error_Category_NotFound()
    {
        var user = UserDtoBuilder.Build();
        var person = PersonBuilder.Build(user);
        var transaction = TransactionBuilder.Build(person);
        var request = TransactionCommandBuilder.Build();

        var useCase = CreateUseCase(user, person, transaction, bankAccountId: request.BankAccountId);

        var exception = await Should.ThrowAsync<NotFoundException>(() => useCase.Execute(transactionId: transaction.Id, request));

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_NOT_FOUND_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.CATEGORY_NOT_FOUND);
        exception.GetStatusCode().ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Error_Amount_Zero()
    {
        var user = UserDtoBuilder.Build();
        var person = PersonBuilder.Build(user);
        var transaction = TransactionBuilder.Build(person);
        var request = TransactionCommandBuilder.Build();
        request.Amount = 0m;

        var useCase = CreateUseCase(user, person, bankAccountId: request.BankAccountId, categoryId: request.CategoryId);

        var exception = await Should.ThrowAsync<ErrorOnValidationException>(() => useCase.Execute(transactionId: transaction.Id, request));

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_DETAIL);

        var error = exception.GetErrors().ShouldHaveSingleItem();
        error.Key.ShouldBe(nameof(request.Amount));

        var value = error.Value.ShouldHaveSingleItem();
        value.ShouldBe(ResourceMessagesException.TRANSACTION_AMOUNT_OUT_OF_RANGE);
    }

    [Fact]
    public async Task Error_Amount_AboveMaximum()
    {
        var user = UserDtoBuilder.Build();
        var person = PersonBuilder.Build(user);
        var transaction = TransactionBuilder.Build(person);
        var request = TransactionCommandBuilder.Build();
        request.Amount = DomainRules.MaximumMoneyAmount + 1m;
        var useCase = CreateUseCase(user, person, bankAccountId: request.BankAccountId, categoryId: request.CategoryId);

        var exception = await Should.ThrowAsync<ErrorOnValidationException>(() => useCase.Execute(transactionId: transaction.Id, request));

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_DETAIL);

        var error = exception.GetErrors().ShouldHaveSingleItem();
        error.Key.ShouldBe(nameof(request.Amount));

        var value = error.Value.ShouldHaveSingleItem();
        value.ShouldBe(ResourceMessagesException.TRANSACTION_AMOUNT_OUT_OF_RANGE);
    }

    [Fact]
    public async Task Error_Date_Default()
    {
        var user = UserDtoBuilder.Build();
        var person = PersonBuilder.Build(user);
        var transaction = TransactionBuilder.Build(person);
        var request = TransactionCommandBuilder.Build();
        request.Date = default;
        var useCase = CreateUseCase(user, person, bankAccountId: request.BankAccountId, categoryId: request.CategoryId);

        var exception = await Should.ThrowAsync<ErrorOnValidationException>(() => useCase.Execute(transactionId: transaction.Id, request));

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_DETAIL);

        var error = exception.GetErrors().ShouldHaveSingleItem();
        error.Key.ShouldBe(nameof(request.Date));

        var value = error.Value.ShouldHaveSingleItem();
        value.ShouldBe(ResourceMessagesException.TRANSACTION_DATE_REQUIRED);
    }

    [Fact]
    public async Task Error_Person_NotFound()
    {
        var user = UserDtoBuilder.Build();
        var request = TransactionCommandBuilder.Build();
        var useCase = CreateUseCase(user);

        var exception = await Should.ThrowAsync<PersonNotFoundException>(useCase.Execute(transactionId: Guid.CreateVersion7(), request: request));

        exception.GetErrorTitle().ShouldBe(ResourceMessagesException.EXCEPTION_PERSON_NOT_FOUND_TITLE);
        exception.GetErrorDetail().ShouldBe(ResourceMessagesException.EXCEPTION_PERSON_NOT_FOUND_DETAIL);
        exception.GetStatusCode().ShouldBe(HttpStatusCode.NotFound);
    }

    private static UpdateTransactionUseCase CreateUseCase(
        UserDto user,
        Gafel.Domain.Entities.Person? person = null,
        Gafel.Domain.Entities.Transaction? transaction = null,
        Guid? bankAccountId = null,
        Guid? categoryId = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var currentUser = CurrentUserBuilder.Build(user);

        var transactionUpdateOnlyRepository = new TransactionUpdateOnlyRepositoryBuilder();
        var bankAccountReadOnlyRepository = new BankAccountReadOnlyRepositoryBuilder();
        var categoryReadOnlyRepository = new CategoryReadOnlyRepositoryBuilder();

        var personReadOnlyRepository = new PersonReadOnlyRepositoryBuilder();
        if (person is not null)
        {
            personReadOnlyRepository.GetByUserId(person);

            if (transaction is not null)
                transactionUpdateOnlyRepository.GetById(person, transaction);

            if (bankAccountId.HasValue)
                bankAccountReadOnlyRepository.ExistActiveBankAccountWithId(person, bankAccountId.Value);

            if (categoryId.HasValue)
                categoryReadOnlyRepository.ExistActiveCategoryWithId(person, categoryId.Value);
        }

        return new(
            currentUser,
            personReadOnlyRepository.Build(),
            transactionUpdateOnlyRepository.Build(),
            bankAccountReadOnlyRepository.Build(),
            categoryReadOnlyRepository.Build(),
            unitOfWork
        );
    }
}
