namespace WebApi.Services;

public class ServiceResult
{
    public bool Success { get; init; }
    public string Description { get; init; } = string.Empty;
    public ServiceResult? InnerServiceResult { get; init; }
    public ServiceResult(bool success, string description)
    {
        Success = success;
        Description = description;
    }
    public ServiceResult(bool success, string description, ServiceResult innerServiceResult) : this (success, description)
    {
        InnerServiceResult = innerServiceResult;
    }

    #region Статические методы.
    public static ServiceResult Fail(string description)
    {
        return new ServiceResult(false, description);
    }
    public static ServiceResult Ok(string description)
    {
        return new ServiceResult(true, description);
    }

    public static ServiceResult Fail(string description, ServiceResult innerServiceResult)
    {
        return new ServiceResult(false, description, innerServiceResult);
    }
    public static ServiceResult Ok(string description, ServiceResult innerServiceResult)
    {
        return new ServiceResult(true, description, innerServiceResult);
    }
    #endregion

    public override string ToString()
    {
        return $"Success: {Success}, Description: {Description}, InnverServiceResult: {InnerServiceResult}";
    }
}
public class ServiceResult<T> : ServiceResult
{
    public T? Value { get; init; }
    public ServiceResult(bool success, string description, T? value) : base(success, description)
    {
        Value = value;
    }

    public ServiceResult(bool success, string description, ServiceResult innerServiceResult, T? value) : base(success, description, innerServiceResult)
    {
        Value = value;
    }

    #region Статические методы.
    public static ServiceResult<T> Fail(string description, T? value)
    {
        return new ServiceResult<T>(false, description, value);
    }
    public static ServiceResult<T> Ok(string description, T? value)
    {
        return new ServiceResult<T>(true, description, value);
    }

    public static ServiceResult<T> Fail(string description, ServiceResult innerServiceResult, T? value)
    {
        return new ServiceResult<T>(false, description, innerServiceResult, value);
    }
    public static ServiceResult<T> Ok(string description, ServiceResult innerServiceResult, T? value)
    {
        return new ServiceResult<T>(true, description, innerServiceResult, value);
    }
    #endregion

    public override string ToString()
    {
        return $"{base.ToString()}, Value: {Value}";
    }
}