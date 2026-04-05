using Gafel.Domain.Resources;
using Shouldly;
using System.Net;
using System.Text.Json;

namespace WebAPI.Test.Assertions;

public static class HttpResponseValidationAssertions
{
    public static async Task ShouldHaveSingleValidationError(this HttpResponseMessage response, string field, string expectedMessage)
    {
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var stream = await response.Content.ReadAsStreamAsync();
        var json = await JsonDocument.ParseAsync(stream);

        json.RootElement.GetProperty("title").GetString()
            .ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_TITLE);

        json.RootElement.GetProperty("detail").GetString()
            .ShouldBe(ResourceMessagesException.EXCEPTION_ERROR_ON_VALIDATION_DETAIL);

        json.RootElement.GetProperty("status").GetInt32()
            .ShouldBe((int)HttpStatusCode.BadRequest);

        var error = json.RootElement
            .GetProperty("errors")
            .EnumerateObject()
            .ShouldHaveSingleItem();

        error.Name.ShouldBe(field);

        error.Value
            .EnumerateArray()
            .ShouldHaveSingleItem()
            .GetString()
            .ShouldBe(expectedMessage);
    }
}
