using BSMed.Shared;
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

    public async Task<ApiResponse<PatientRegistrationDto>> RegisterPatientAsync(PatientRegisterInput model)
    {
        try
        {
            // Get current auth token if exists
            var authToken = await GetAuthTokenAsync();

            var response = await _apiClient.PostAsJsonAsync<PatientRegisterInput, PatientRegistrationDto>(
                "api/patient/register",
                model,
                authToken);

            return response;
        }
        catch (ApiException ex)
        {
            return ApiResponse<PatientRegistrationDto>.Fail(ex.Message);
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

    public async Task<RegistrationResult> SelfRegisterPatientAsync(PatientRegisterInput model)
    {
        try
        {
            var response = await _apiClient.PostAsJsonAsync<PatientRegisterInput, AuthResponse>(
                "api/patient/register-patient",
                model);

            if (!response.Success)
            {
                return new RegistrationResult
                {
                    Success = false,
                    ErrorMessage = response.Error ?? "Registration failed"
                };
            }

            return new RegistrationResult
            {
                Success = true
            };
        }
        catch (ApiException ex)
        {
            //var errorResponse = ex.GetContentAs<BaseApiResponse<AuthResponse>>();
            return new RegistrationResult
            {
                Success = false,
                ErrorMessage = ex.Message,
                //Errors = errorResponse?.Errors ?? new List<string>()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Patient registration failed");
            return new RegistrationResult
            {
                Success = false,
                ErrorMessage = "An unexpected error occurred"
            };
        }
    }
}

