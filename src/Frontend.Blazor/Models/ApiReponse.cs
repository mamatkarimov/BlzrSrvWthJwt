
namespace Frontend.Blazor.Models;

public class ApiResponse<T>
{
    public T Result { get; set; }
    public List<string> Errors { get; set; }
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
                Errors = new List<string> {e.Message}
            };
        }
    }

    public bool Success { get; set; }

    public string Error { get; set; }

    public List<string> ValidationErrors { get; set; } = new();

    public static ApiResponse<T> Ok(T result)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Result = result,
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
            ValidationErrors = validationErrors
        };
    }
}



// Non-generic version for simple responses
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