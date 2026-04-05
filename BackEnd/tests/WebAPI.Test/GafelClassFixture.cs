using System.Net.Http.Json;

namespace WebAPI.Test;

public class GafelClassFixture(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient = factory.CreateClient();


    protected async Task<HttpResponseMessage> DoGet(string method, string? token = null)
    {
        AuthorizeRequest(token);

        return await _httpClient.GetAsync(method);
    }

    protected async Task<HttpResponseMessage> DoPost(string method, object request, string? token = null)
    {
        AuthorizeRequest(token);

        return await _httpClient.PostAsJsonAsync(method, request);
    }

    protected async Task<HttpResponseMessage> DoPut(string method, object request, string? token = null)
    {
        AuthorizeRequest(token);

        return await _httpClient.PutAsJsonAsync(method, request);
    }

    protected async Task<HttpResponseMessage> DoDelete(string method, string? token = null)
    {
        AuthorizeRequest(token);

        return await _httpClient.DeleteAsync(method);
    }


    private void AuthorizeRequest(string? token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return;

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }
}
