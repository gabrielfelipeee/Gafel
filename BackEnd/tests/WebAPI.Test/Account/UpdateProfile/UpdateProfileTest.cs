using CommonTestUtilities.Commands;
using CommonTestUtilities.Tokens;
using Gafel.Application.UseCases.Auth.SharedResponses;
using Gafel.Domain.Resources;
using Shouldly;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace WebAPI.Test.Account.UpdateProfile;

public class UpdateProfileTest : GafelClassFixture
{
    private const string METHOD = "me";

    private readonly string _personCpf;
    private readonly long _userId;

    public UpdateProfileTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _personCpf = factory.GetPersonCpf();
        _userId = factory.GetUserId();
    }

    [Fact]
    public async Task Success()
    {
        // Arrange
        var request = UpdateProfileCommandBuilder.Build();
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _userId);

        // Act
        var response = await DoPut(method: METHOD, request: request, token: token);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Error_Cpf_Invalid()
    {
        var request = UpdateProfileCommandBuilder.Build();
        request.Cpf = "00000000000";
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _userId);


        var response = await DoPut(method: METHOD, request: request, token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var title = responseData.RootElement.GetProperty("title").GetString();
        title.ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_TITLE);

        var detail = responseData.RootElement.GetProperty("detail").GetString();
        detail.ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_DETAIL);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateObject().ToList();
        var error = errors.ShouldHaveSingleItem();
        error.Name.ShouldBe("cpf");

        var errorMessages = error.Value.EnumerateArray().ToList();
        var message = errorMessages.ShouldHaveSingleItem().GetString();
        message.ShouldBe(ResourceMessagesException.CPF_INVALID);
    }

    [Fact]
    public async Task Error_Cpf_Already_Registered_By_Another_User()
    {
        // Cria nova conta
        var requestRegister = RegisterAccountCommandBuilder.Build();

        var responseRegister = await DoPost(method: "auth/register", request: requestRegister);

        var authResponse = await responseRegister.Content.ReadFromJsonAsync<AuthResponse>();

        var token = authResponse!.Tokens.AccessToken;


        // Tenta atualizar a nova pessoa com um CPF existente no banco
        var request = UpdateProfileCommandBuilder.Build();
        request.Cpf = _personCpf;

        var response = await DoPut(method: METHOD, request: request, token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var title = responseData.RootElement.GetProperty("title").GetString();
        title.ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_TITLE);

        var detail = responseData.RootElement.GetProperty("detail").GetString();
        detail.ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_DETAIL);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateObject().ToList();
        var error = errors.ShouldHaveSingleItem();
        error.Name.ShouldBe("cpf");

        var errorMessages = error.Value.EnumerateArray().ToList();
        var message = errorMessages.ShouldHaveSingleItem().GetString();
        message.ShouldBe(ResourceMessagesException.CPF_ALREADY_REGISTERED);
    }

    [Fact]
    public async Task Error_Cpf_Cannot_Be_Modified_After_Creation()
    {
        var request = UpdateProfileCommandBuilder.Build(withCpf: true);
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _userId);

        var response = await DoPut(method: METHOD, request: request, token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var title = responseData.RootElement.GetProperty("title").GetString();
        title.ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_TITLE);

        var detail = responseData.RootElement.GetProperty("detail").GetString();
        detail.ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_DETAIL);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateObject().ToList();
        var error = errors.ShouldHaveSingleItem();
        error.Name.ShouldBe("cpf");

        var errorMessages = error.Value.EnumerateArray().ToList();
        var message = errorMessages.ShouldHaveSingleItem().GetString();
        message.ShouldBe(ResourceMessagesException.CPF_UPDATE_NOT_ALLOWED);
    }

    [Fact]
    public async Task Error_DateOfBirth_Invalid()
    {
        var request = UpdateProfileCommandBuilder.Build(withDateOfBirth: true);
        request.DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow);
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _userId);

        var response = await DoPut(method: METHOD, request: request, token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var title = responseData.RootElement.GetProperty("title").GetString();
        title.ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_TITLE);

        var detail = responseData.RootElement.GetProperty("detail").GetString();
        detail.ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_DETAIL);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateObject().ToList();
        var error = errors.ShouldHaveSingleItem();
        error.Name.ShouldBe("dateOfBirth");

        var errorMessages = error.Value.EnumerateArray().ToList();
        var message = errorMessages.ShouldHaveSingleItem().GetString();
        message.ShouldBe(ResourceMessagesException.PERSON_DATE_OF_BIRTH_INVALID);
    }

    [Fact]
    public async Task Error_DateOfBirth_Future()
    {
        var request = UpdateProfileCommandBuilder.Build(withDateOfBirth: true);
        request.DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5));
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _userId);

        var response = await DoPut(method: METHOD, request: request, token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var title = responseData.RootElement.GetProperty("title").GetString();
        title.ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_TITLE);

        var detail = responseData.RootElement.GetProperty("detail").GetString();
        detail.ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_DETAIL);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateObject().ToList();
        var error = errors.ShouldHaveSingleItem();
        error.Name.ShouldBe("dateOfBirth");

        var errorMessages = error.Value.EnumerateArray().ToList();
        var message = errorMessages.ShouldHaveSingleItem().GetString();
        message.ShouldBe(ResourceMessagesException.PERSON_DATE_OF_BIRTH_FUTURE);
    }

    [Fact]
    public async Task Error_Uf_Invalid()
    {
        var request = UpdateProfileCommandBuilder.Build();
        request.Uf = "XX";
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _userId);

        var response = await DoPut(method: METHOD, request: request, token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var title = responseData.RootElement.GetProperty("title").GetString();
        title.ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_TITLE);

        var detail = responseData.RootElement.GetProperty("detail").GetString();
        detail.ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_DETAIL);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateObject().ToList();
        var error = errors.ShouldHaveSingleItem();
        error.Name.ShouldBe("uf");

        var errorMessages = error.Value.EnumerateArray().ToList();
        var message = errorMessages.ShouldHaveSingleItem().GetString();
        message.ShouldBe(ResourceMessagesException.PERSON_UF_INVALID);
    }

    [Fact]
    public async Task Error_FullName_Empty()
    {
        var request = UpdateProfileCommandBuilder.Build();
        request.FullName = string.Empty;
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _userId);

        var response = await DoPut(method: METHOD, request: request, token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var title = responseData.RootElement.GetProperty("title").GetString();
        title.ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_TITLE);

        var detail = responseData.RootElement.GetProperty("detail").GetString();
        detail.ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_DETAIL);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateObject().ToList();
        var error = errors.ShouldHaveSingleItem();
        error.Name.ShouldBe("fullName");

        var errorMessages = error.Value.EnumerateArray().ToList();
        var message = errorMessages.ShouldHaveSingleItem().GetString();
        message.ShouldBe(ResourceMessagesException.PERSON_FULL_NAME_EMPTY);
    }

    [Fact]
    public async Task Error_City_Length_Equal_To_2()
    {
        var request = UpdateProfileCommandBuilder.Build();
        request.City = "Sp";
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _userId);

        var response = await DoPut(method: METHOD, request: request, token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var title = responseData.RootElement.GetProperty("title").GetString();
        title.ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_TITLE);

        var detail = responseData.RootElement.GetProperty("detail").GetString();
        detail.ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_DETAIL);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateObject().ToList();
        var error = errors.ShouldHaveSingleItem();
        error.Name.ShouldBe("city");

        var errorMessages = error.Value.EnumerateArray().ToList();
        var message = errorMessages.ShouldHaveSingleItem().GetString();
        message.ShouldBe(ResourceMessagesException.PERSON_CITY_INVALID);
    }
}
