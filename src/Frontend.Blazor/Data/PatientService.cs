using Frontend.Blazor.HttpClients;
using Frontend.Blazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using static Frontend.Blazor.Pages.RegisterPatient;

namespace Frontend.Blazor.Data;

public class PatientService
{
    private const string AccessToken = nameof(AccessToken);
    private readonly IBackendApiHttpClient _apiClient;
    private readonly ProtectedLocalStorage _localStorage;
    private readonly NavigationManager _navigation;
    private readonly ILogger<PatientService> _logger;

    public PatientService(
        IBackendApiHttpClient apiClient,
        ProtectedLocalStorage localStorage,
        NavigationManager navigation,
        ILogger<PatientService> logger)
    {
        _apiClient = apiClient;
        _localStorage = localStorage;
        _navigation = navigation;
        _logger = logger;
    }

    public async Task<RegistrationResult> RegisterPatientAsync(PatientRegisterInput model)
    {
        try
        {
            // Get current auth token if exists
            var authToken = await GetAuthTokenAsync();

            var response = await _apiClient.PostAsJsonAsync<PatientRegisterInput, AuthResponse>(
                "api/patient/register-patient",
                model,
                authToken);

            if (!response.Success)
            {
                return new RegistrationResult
                {
                    Success = false,
                    ErrorMessage = response.Error,
                    RequiresLogin = response.Error?.Contains("login") ?? false
                };
            }

            // Store new tokens if registration returns them
            if (!string.IsNullOrEmpty(response.Result?.JwtToken))
            {
                await _localStorage.SetAsync(AccessToken, response.Result.JwtToken);
            }

            return new RegistrationResult { Success = true };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Patient registration failed");
            return new RegistrationResult
            {
                Success = false,
                ErrorMessage = ex.Message,
                RequiresLogin = ex is UnauthorizedAccessException
            };
        }
    }

    private async Task<string> GetAuthTokenAsync()
    {
        try
        {
            var tokenResult = await _localStorage.GetAsync<string>(AccessToken);
            return tokenResult.Success ? tokenResult.Value : null;
        }
        catch
        {
            return null;
        }
    }
}

public class RegistrationResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; }
    public bool RequiresLogin { get; set; }
}