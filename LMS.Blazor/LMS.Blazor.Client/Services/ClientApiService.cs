using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.Text.Json;

namespace LMS.Blazor.Client.Services;

public class ClientApiService : IApiService
{
    private readonly HttpClient _httpClient;
    private readonly NavigationManager _navigationManager;
    private readonly JsonSerializerOptions _jsonOptions;

    public ClientApiService(HttpClient httpClient, NavigationManager navigationManager)
    {
        _httpClient = httpClient;

        _navigationManager = navigationManager;

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public async Task<T?> GetAsync<T>(string endpoint, CancellationToken ct = default)
    {
        var response = await _httpClient.GetAsync($"api/proxy/{endpoint}", ct);
        HandleUnauthorized(response);
        response.EnsureSuccessStatusCode();
        return await DeserializeAsync<T>(response, ct);
    }

    public async Task<T?> PostAsync<T>(string endpoint, object body, CancellationToken ct = default)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/proxy/{endpoint}", body, _jsonOptions, ct);
        HandleUnauthorized(response);
        response.EnsureSuccessStatusCode();
        return await DeserializeAsync<T>(response, ct);
    }

    public async Task<T?> PutAsync<T>(string endpoint, object body, CancellationToken ct = default)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/proxy/{endpoint}", body, _jsonOptions, ct);
        HandleUnauthorized(response);
        response.EnsureSuccessStatusCode();
        return await DeserializeAsync<T>(response, ct);
    }

    public async Task<bool> DeleteAsync(string endpoint, CancellationToken ct = default)
    {
        var response = await _httpClient.DeleteAsync($"api/proxy/{endpoint}", ct);
        HandleUnauthorized(response);
        return response.IsSuccessStatusCode;
    }

    public async Task<T?> PostMultipartAsync<T>(
        string endpoint,
        MultipartFormDataContent content,
        CancellationToken ct = default)
    {
        var response = await _httpClient.PostAsync($"api/proxy/{endpoint}", content, ct);
        HandleUnauthorized(response);
        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync(ct);
            Console.WriteLine(errorBody);
        }
        response.EnsureSuccessStatusCode();
        return await DeserializeAsync<T>(response, ct);
    }

    public async Task<T?> PutMultipartAsync<T>(
        string endpoint,
        MultipartFormDataContent content,
        CancellationToken ct = default)
    {
        var response = await _httpClient.PutAsync($"api/proxy/{endpoint}", content, ct);

        HandleUnauthorized(response);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync(ct);
            Console.WriteLine(errorBody);
        }

        response.EnsureSuccessStatusCode();

        return await DeserializeAsync<T>(response, ct);
    }

    // -------------------------
    // Private helpers
    // -------------------------

    private void HandleUnauthorized(HttpResponseMessage response)
    {
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
            response.StatusCode == System.Net.HttpStatusCode.Forbidden)
        {
            _navigationManager.NavigateTo("/Account/Login", forceLoad: true);
        }
    }

    //private async Task<T?> DeserializeAsync<T>(HttpResponseMessage response, CancellationToken ct)
    //{
    //    return await JsonSerializer.DeserializeAsync<T>(await response.Content.ReadAsStreamAsync(ct), _jsonOptions, ct);
    //}
    private async Task<T?> DeserializeAsync<T>(HttpResponseMessage response, CancellationToken ct)
    {
        if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            return default;

        var content = await response.Content.ReadAsStreamAsync(ct);
        if (content.Length == 0)
            return default;

        return await JsonSerializer.DeserializeAsync<T>(content, _jsonOptions, ct);
    }


}
