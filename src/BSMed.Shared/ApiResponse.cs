namespace BSMed.Shared;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T Data { get; set; }
    public string Error { get; set; }
    public List<string> Errors { get; set; } = new List<string>();

    public ApiResponse() { }

    public ApiResponse(T data)
    {
        Success = true;
        Data = data;
    }

    public ApiResponse(string error)
    {
        Success = false;
        Error = error;
    }

    public ApiResponse(IEnumerable<string> errors)
    {
        Success = false;
        Errors = errors.ToList();
    }

    public static ApiResponse<T> FromModelState(Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelState)
    {
        var errors = modelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

        return new ApiResponse<T>(errors);
    }

    public static ApiResponse<T> FromException(System.Exception ex)
    {
        return new ApiResponse<T>(ex.Message);
    }

    public static ApiResponse<T> FromErrors(IEnumerable<string> errors)
    {
        return new ApiResponse<T>(errors);
    }

    public static async Task<ApiResponse<T>> HandleExceptionAsync(Func<Task<ApiResponse<T>>> action)
    {
        try
        {
            var result = await action();
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new ApiResponse<T>
            {
                Errors = new List<string> { e.Message }
            };
        }
    }

    public static ApiResponse<T> Ok(T result)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Data = result,
            Error = null
        };
    }

    public static ApiResponse<T> Fail(string errorMessage)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Error = errorMessage
        };
    }

    public static ApiResponse<T> Fail(string errorMessage, List<string> validationErrors)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Error = errorMessage,
            Errors = validationErrors
        };
    }
}

public class ApiResponse
{
    public bool Success { get; set; }
    public string Error { get; set; }
    public List<string> ValidationErrors { get; set; } = new();

    public static ApiResponse Ok()
    {
        return new ApiResponse { Success = true };
    }

    public static ApiResponse Fail(string errorMessage)
    {
        return new ApiResponse { Success = false, Error = errorMessage };
    }
}
