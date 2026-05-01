using CommonTestUtilities.Commands;
using CommonTestUtilities.Tokens;
using Gafel.Application.UseCases.Auth.SharedResponses;
using Gafel.Domain.Dtos;
using Gafel.Domain.Resources;
using Shouldly;
using System.Net;
using System.Net.Http.Json;
using WebAPI.Test.Assertions;

namespace WebAPI.Test.Account.UpdateProfile;

public class UpdateProfileTest : GafelClassFixture
{
    private const string METHOD = "me";

    private readonly UserDto _user;
    private readonly Gafel.Domain.Entities.Person _person;

    public UpdateProfileTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _user = factory.GetUser();
        _person = factory.GetPerson();
    }

    [Fact]
    public async Task Success()
    {
        // Arrange
        var request = UpdateProfileCommandBuilder.Build();
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);

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
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);


        var response = await DoPut(method: METHOD, request: request, token: token);

        await response.ShouldHaveSingleValidationError(field: "cpf", expectedMessage: ResourceMessagesException.CPF_INVALID);
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
        request.Cpf = _person.Cpf!.Value;

        var response = await DoPut(method: METHOD, request: request, token: token);

        await response.ShouldHaveSingleValidationError(field: "cpf", expectedMessage: ResourceMessagesException.CPF_ALREADY_REGISTERED);
    }

    [Fact]
    public async Task Error_Cpf_Cannot_Be_Modified_After_Creation()
    {
        var request = UpdateProfileCommandBuilder.Build(withCpf: true);
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);

        var response = await DoPut(method: METHOD, request: request, token: token);

        await response.ShouldHaveSingleValidationError(field: "cpf", expectedMessage: ResourceMessagesException.CPF_UPDATE_NOT_ALLOWED);
    }

    [Fact]
    public async Task Error_DateOfBirth_Invalid()
    {
        var request = UpdateProfileCommandBuilder.Build(withDateOfBirth: true);
        request.DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow);
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);

        var response = await DoPut(method: METHOD, request: request, token: token);

        await response.ShouldHaveSingleValidationError(field: "dateOfBirth", expectedMessage: ResourceMessagesException.PERSON_DATE_OF_BIRTH_INVALID);
    }

    [Fact]
    public async Task Error_DateOfBirth_Future()
    {
        var request = UpdateProfileCommandBuilder.Build(withDateOfBirth: true);
        request.DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5));
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);

        var response = await DoPut(method: METHOD, request: request, token: token);

        await response.ShouldHaveSingleValidationError(field: "dateOfBirth", expectedMessage: ResourceMessagesException.PERSON_DATE_OF_BIRTH_FUTURE);
    }

    [Fact]
    public async Task Error_Uf_Invalid()
    {
        var request = UpdateProfileCommandBuilder.Build();
        request.Uf = "XX";
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);

        var response = await DoPut(method: METHOD, request: request, token: token);

        await response.ShouldHaveSingleValidationError(field: "uf", expectedMessage: ResourceMessagesException.PERSON_UF_INVALID);
    }

    [Fact]
    public async Task Error_FullName_Empty()
    {
        var request = UpdateProfileCommandBuilder.Build();
        request.FullName = string.Empty;
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);

        var response = await DoPut(method: METHOD, request: request, token: token);

        await response.ShouldHaveSingleValidationError(field: "fullName", expectedMessage: ResourceMessagesException.PERSON_FULL_NAME_EMPTY);
    }

    [Fact]
    public async Task Error_City_Length_Equal_To_2()
    {
        var request = UpdateProfileCommandBuilder.Build();
        request.City = "Sp";
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);

        var response = await DoPut(method: METHOD, request: request, token: token);

        await response.ShouldHaveSingleValidationError(field: "city", expectedMessage: ResourceMessagesException.PERSON_CITY_INVALID);
    }
}
