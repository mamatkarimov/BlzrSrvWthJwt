using System.Net;

namespace BSMed.Shared;

public class ApiException : Exception
{
    public HttpStatusCode StatusCode { get; }
    public string ErrorDetails { get; }
    public string ErrorCode { get; }

    public ApiException(
        string message,
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError,
        string errorDetails = null,
        string errorCode = null)
        : base(message)
    {
        StatusCode = statusCode;
        ErrorDetails = errorDetails;
        ErrorCode = errorCode;
    }

    public ApiException(
        Exception innerException,
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError,
        string errorDetails = null,
        string errorCode = null)
        : base(innerException.Message, innerException)
    {
        StatusCode = statusCode;
        ErrorDetails = errorDetails ?? innerException.Message;
        ErrorCode = errorCode;
    }

}

// 400 Bad Request
public class BadRequestException : ApiException
{
    public BadRequestException(string message, string errorDetails = null)
        : base(message, HttpStatusCode.BadRequest, errorDetails, "BAD_REQUEST") { }
}

// 401 Unauthorized
public class UnauthorizedException : ApiException
{
    public UnauthorizedException(string message)
        : base(message, HttpStatusCode.Unauthorized, null, "UNAUTHORIZED") { }
}

// 403 Forbidden
public class ForbiddenException : ApiException
{
    public ForbiddenException(string message)
        : base(message, HttpStatusCode.Forbidden, null, "FORBIDDEN") { }
}

// 404 Not Found
public class NotFoundException : ApiException
{
    public NotFoundException(string message)
        : base(message, HttpStatusCode.NotFound, null, "NOT_FOUND") { }
}

// 409 Conflict (e.g., duplicate entry)
public class ConflictException : ApiException
{
    public ConflictException(string message, string errorDetails = null)
        : base(message, HttpStatusCode.Conflict, errorDetails, "CONFLICT") { }
}

// 422 Unprocessable Entity (validation errors)
public class ValidationException : ApiException
{
    public ValidationException(string message, string errorDetails = null)
        : base(message, HttpStatusCode.UnprocessableEntity, errorDetails, "VALIDATION_ERROR") { }
}
