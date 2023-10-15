namespace WebApi.Services;

public class ServiceResult
{
    public bool Success { get; init; }
    public string Description = string.Empty;
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
