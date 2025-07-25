using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Backend.API.Models;

public class AuthResponse
{
    public bool Success { get; set; }
    public string JwtToken { get; set; }
    public string RefreshToken { get; set; }
    public string UserId { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public IList<string> Roles { get; set; } = new List<string>();
    public string Error { get; set; }
    public bool RequiresEmailVerification { get; set; }
    public bool IsLockedOut { get; set; }
    public bool IsNotAllowed { get; set; }
    public bool RequiresTwoFactor { get; set; }

    public static AuthResponse SuccessResponse(string token, string refreshToken, string userId,
        string email, string userName, IList<string> roles)
    {
        return new AuthResponse
        {
            Success = true,
            JwtToken = token,
            RefreshToken = refreshToken,
            UserId = userId,
            Email = email,
            UserName = userName,
            Roles = roles
        };
    }

    public static AuthResponse FailureResponse(string error)
    {
        return new AuthResponse
        {
            Success = false,
            Error = error
        };
    }

    public static AuthResponse RequiresEmailVerificationResponse()
    {
        return new AuthResponse
        {
            Success = false,
            RequiresEmailVerification = true,
            Error = "Email verification required"
        };
    }

    public static AuthResponse LockedOutResponse()
    {
        return new AuthResponse
        {
            Success = false,
            IsLockedOut = true,
            Error = "Account locked out"
        };
    }

    public static AuthResponse NotAllowedResponse()
    {
        return new AuthResponse
        {
            Success = false,
            IsNotAllowed = true,
            Error = "Account not allowed to login"
        };
    }

    public static AuthResponse RequiresTwoFactorResponse()
    {
        return new AuthResponse
        {
            Success = false,
            RequiresTwoFactor = true,
            Error = "Two-factor authentication required"
        };
    }
}


