namespace BasicDotnetTemplate.MainProject.Models.Api.Response;

public class BaseResponse
{
    public int Status { get; set; }
    public string Message { get; set; }
#nullable enable
    public object? Data { get; set; }
#nullable disable
}