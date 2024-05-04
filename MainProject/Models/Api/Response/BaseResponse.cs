namespace BasicDotnetTemplate.MainProject.Models.Api.Response;

public class BaseResponse
{
    public int Status { get; set; }
#nullable enable
    public string? Message { get; set; }

    public object? Data { get; set; }
#nullable disable
}