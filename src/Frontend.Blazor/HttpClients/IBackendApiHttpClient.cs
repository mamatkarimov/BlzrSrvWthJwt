using BSMed.Shared;
using Frontend.Blazor.Models;

namespace Frontend.Blazor.HttpClients;

public interface IBackendApiHttpClient
{
    Task<ApiResponse<string>> RegisterUserAsync(UserRegisterInput model, CancellationToken? cancellationToken = null);
    Task<ApiResponse<AuthResponse>> LoginUserAsync(LoginModel model, CancellationToken? cancellationToken = null);

    Task<ApiResponse<AuthResponse>> RefreshTokenAsync(string refreshToken,
        CancellationToken? cancellationToken = null);
    Task<ApiResponse<TResponse>> PostAsJsonAsync<TRequest, TResponse>(string url, TRequest data, string authToken = null, CancellationToken? cancellationToken = null);
}
