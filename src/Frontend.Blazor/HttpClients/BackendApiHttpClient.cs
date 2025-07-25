using BSMed.Shared;
using Frontend.Blazor.Models;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Frontend.Blazor.HttpClients;

public class BackendApiHttpClient: IBackendApiHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<BackendApiHttpClient> _logger;

    public BackendApiHttpClient(HttpClient httpClient, IConfiguration configuration, ILogger<BackendApiHttpClient> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;

        //_httpClient.BaseAddress = new Uri(_configuration["BackendAPI"]);
        _logger = logger;
    }

    public async Task<ApiResponse<string>> RegisterUserAsync(UserRegisterInput model, CancellationToken? cancellationToken = null)
    {
        return await ApiResponse<string>.HandleExceptionAsync(async () =>
        {
            var response =
                await _httpClient.PostAsJsonAsync("api/account", model, cancellationToken ?? CancellationToken.None);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<ApiResponse<string>>(cancellationToken ??
                CancellationToken.None);
        });
    }
    public async Task<ApiResponse<AuthResponse>> LoginUserAsync(LoginModel model, CancellationToken? cancellationToken = null)
    {
        return await ApiResponse<AuthResponse>.HandleExceptionAsync(async () =>
        {
            var response = await _httpClient.PostAsJsonAsync("api/account/login", model);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponse>>(cancellationToken ?? CancellationToken.None);
        });
    }
    public async Task<ApiResponse<AuthResponse>> RefreshTokenAsync(string refreshToken, CancellationToken? cancellationToken = null)
    {
        return await ApiResponse<AuthResponse>.HandleExceptionAsync(async () =>
        {
            var response = await _httpClient.PostAsJsonAsync("api/account/refresh", new{ refreshToken });

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponse>>(cancellationToken ?? CancellationToken.None);
        });
    }

    public async Task<ApiResponse<TResponse>> PostAsJsonAsync<TRequest, TResponse>(
    string url,
    TRequest data,
    string authToken = null,
    CancellationToken? cancellationToken = null)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(data)
            };

            // Add auth header if token is provided
            if (!string.IsNullOrEmpty(authToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            }

            var response = await _httpClient.SendAsync(
                request,
                cancellationToken ?? CancellationToken.None);

            // Special handling for 401 Unauthorized
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _logger.LogWarning("Unauthorized access attempt to {Url}", url);
                return ApiResponse<TResponse>.Fail("Session expired. Please login again.");
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("API Error {StatusCode}: {Response}",
                    response.StatusCode, responseContent);

                try
                {
                    var errorResponse = JsonSerializer.Deserialize<ApiResponse<TResponse>>(responseContent);
                    return errorResponse ?? ApiResponse<TResponse>.Fail($"Error: {response.StatusCode}");
                }
                catch
                {
                    return ApiResponse<TResponse>.Fail(await GetErrorMessage(response));
                }
            }

            return JsonSerializer.Deserialize<ApiResponse<TResponse>>(responseContent)
                   ?? ApiResponse<TResponse>.Fail("Invalid response from server");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Request failed to {Url}", url);
            return ApiResponse<TResponse>.Fail(GetUserFriendlyError(ex));
        }
    }

    private async Task<string> GetErrorMessage(HttpResponseMessage response)
    {
        return response.StatusCode switch
        {
            HttpStatusCode.Unauthorized => "Authentication required",
            HttpStatusCode.Forbidden => "Access denied",
            _ => $"Error: {response.StatusCode}"
        };
    }

    private string GetUserFriendlyError(Exception ex)
    {
        return ex switch
        {
            HttpRequestException httpEx when httpEx.StatusCode == HttpStatusCode.Unauthorized
                => "Session expired. Please login again.",
            HttpRequestException httpEx => "Network error occurred",
            TaskCanceledException => "Request timeout",
            _ => "An unexpected error occurred"
        };
    }

    private async Task<ApiResponse<T>> ExecuteApiCallAsync<T>(Func<Task<ApiResponse<T>>> apiCall)
    {
        try
        {
            return await apiCall();
        }
        catch (HttpRequestException ex) when (ex.StatusCode.HasValue)
        {
            _logger.LogError(ex, "HTTP error {StatusCode} during API call", ex.StatusCode);
            return ApiResponse<T>.Fail($"HTTP error: {ex.StatusCode}");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Network error during API call");
            return ApiResponse<T>.Fail("Network error occurred");
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "JSON parsing error during API call");
            return ApiResponse<T>.Fail("Invalid response format");
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogWarning(ex, "API call was canceled");
            return ApiResponse<T>.Fail("Request was canceled");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during API call");
            return ApiResponse<T>.Fail("An unexpected error occurred");
        }
    }

    private async Task<ApiResponse<T>> ProcessResponseAsync<T>(
        HttpResponseMessage response,
        CancellationToken? cancellationToken)
    {
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken ?? CancellationToken.None);

        try
        {
            response.EnsureSuccessStatusCode();

            var result = JsonSerializer.Deserialize<ApiResponse<T>>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (result == null)
            {
                throw new JsonException("Deserialization returned null");
            }

            return result;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "API returned error status: {StatusCode}", response.StatusCode);

            try
            {
                var errorResponse = JsonSerializer.Deserialize<ApiResponse<T>>(responseContent);
                return errorResponse ?? ApiResponse<T>.Fail($"Error: {response.StatusCode}");
            }
            catch (JsonException)
            {
                return ApiResponse<T>.Fail($"Error: {response.StatusCode}");
            }
        }
    }

}