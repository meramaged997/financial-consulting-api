namespace startawy.Application.Common.Models;

public class Result<T>
{
    public bool   IsSuccess        { get; }
    public T?     Value            { get; }
    public string? Error           { get; }
    public IDictionary<string, string[]>? ValidationErrors { get; }

    private Result(T value)                  { IsSuccess = true;  Value = value; }
    private Result(string error, IDictionary<string, string[]>? errors = null)
                                             { IsSuccess = false; Error = error; ValidationErrors = errors; }

    public static Result<T> Success(T value)                                      => new(value);
    public static Result<T> Failure(string error)                                 => new(error);
    public static Result<T> ValidationFailure(IDictionary<string, string[]> errs) => new("Validation failed.", errs);
}

public class Result
{
    public bool    IsSuccess { get; }
    public string? Error     { get; }

    private Result(bool success, string? error = null) { IsSuccess = success; Error = error; }

    public static Result Success()             => new(true);
    public static Result Failure(string error) => new(false, error);
}
