namespace UrlShortener.Domain.Common;

public class Result<T> where T : class
{
    public bool Success { get; private set; } = true;
    public T? Data { get; private set; }
    public List<string> ErrorMessages { get; private set; } = [];

    public Result(T result)
    {
        Data = result;
    }

    public Result(List<string> errorMessages)
    {
        Success = false;
        ErrorMessages = errorMessages;
    }
}