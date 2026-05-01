using CommonTestUtilities.Commands;
using CommonTestUtilities.Tokens;
using Gafel.Domain.Dtos;
using Gafel.Domain.Enums;
using Gafel.Domain.Resources;
using Shouldly;
using System.Net;
using WebAPI.Test.Assertions;

namespace WebAPI.Test.Category.Register;

public class RegisterCategoryTest : GafelClassFixture
{
    private const string METHOD = "categories";

    private readonly UserDto _user;
    private readonly Gafel.Domain.Entities.Person _person;

    public RegisterCategoryTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _user = factory.GetUser();
        _person = factory.GetPerson();
    }

    [Fact]
    public async Task Success()
    {
        // Arrange
        var request = CategoryCommandBuilder.Build();
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);

        // Act
        var response = await DoPost(method: METHOD, request: request, token: token);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = CategoryCommandBuilder.Build();
        request.Name = string.Empty;
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);

        var response = await DoPost(method: METHOD, request: request, token: token);

        await response.ShouldHaveSingleValidationError(field: "name", expectedMessage: ResourceMessagesException.CATEGORY_NAME_EMPTY);
    }

    [Fact]
    public async Task Error_Icon_Empty()
    {
        var request = CategoryCommandBuilder.Build();
        request.Icon = string.Empty;
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);

        var response = await DoPost(method: METHOD, request: request, token: token);

        await response.ShouldHaveSingleValidationError(field: "icon", expectedMessage: ResourceMessagesException.CATEGORY_ICON_EMPTY);
    }

    [Fact]
    public async Task Error_Type_Invalid()
    {
        var request = CategoryCommandBuilder.Build();
        request.Type = (CategoryType)100;
        var token = JwtTokenGeneratorBuilder.Build().Generate(userId: _user.Id);

        var response = await DoPost(method: METHOD, request: request, token: token);

        await response.ShouldHaveSingleValidationError(field: "type", expectedMessage: ResourceMessagesException.CATEGORY_TYPE_INVALID);
    }
}
