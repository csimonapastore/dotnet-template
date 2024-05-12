namespace BasicDotnetTemplate.MainProject.Models.Api.Response;

public class BaseResponse
{
#nullable enable
    public BaseResponse(int status, string? message, dynamic? data)
    {
        this.Status = status;
        this.Message = message;
        this.Data = data;
    }
#nullable disable
    public int Status { get; set; }
#nullable enable
    public string? Message { get; set; }

    public virtual dynamic? Data { get; set; }
#nullable disable
}