namespace Backend.API.Models;

public class BaseServiceResult<T>
{
    public bool Succeeded { get; set; }
    public T Data { get; set; }
    public List<string> Errors { get; set; } = new();

    public static BaseServiceResult<T> Success(T data)
    {
        return new BaseServiceResult<T>
        {
            Succeeded = true,
            Data = data
        };
    }

    public static BaseServiceResult<T> Fail(params string[] errors)
    {
        return new BaseServiceResult<T>
        {
            Succeeded = false,
            Errors = errors.ToList()
        };
    }

    public static BaseServiceResult<T> Fail(IEnumerable<string> errors)
    {
        return new BaseServiceResult<T>
        {
            Succeeded = false,
            Errors = errors.ToList()
        };
    }
}
