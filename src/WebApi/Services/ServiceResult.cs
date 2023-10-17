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
}