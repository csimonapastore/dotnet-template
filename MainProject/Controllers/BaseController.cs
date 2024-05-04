using System.Net;
using Microsoft.AspNetCore.Mvc;
using BasicDotnetTemplate.MainProject.Models.Api.Response;
using BasicDotnetTemplate.MainProject.Models.Settings;

[Controller]
public abstract class BaseController : ControllerBase
{
    protected readonly IConfiguration _configuration;
    protected readonly AppSettings _appSettings;

    public BaseController(
        IConfiguration configuration
    )
    {
        _configuration = configuration;
        _appSettings = new AppSettings();
        _configuration.GetSection("AppSettings").Bind(_appSettings);
    }

#nullable enable
    private BaseResponse CreateResponse(HttpStatusCode status, string message, object? data = null)
    {
        return new BaseResponse()
        {
            Status = (int)status,
            Message = message,
            Data = data
        };
    }

    protected new IActionResult Created(string message, object? data = null)
    {
        return StatusCode((int)HttpStatusCode.Created, CreateResponse(HttpStatusCode.Created, message, data));
    }

    protected IActionResult Success(string message, object? data = null)
    {
        return StatusCode((int)HttpStatusCode.OK, CreateResponse(HttpStatusCode.OK, message, data));
    }

    protected IActionResult NotFound(string message, object? data = null)
    {
        return StatusCode((int)HttpStatusCode.NotFound, CreateResponse(HttpStatusCode.NotFound, message, data));
    }

    protected IActionResult BadRequest(string message, object? data = null)
    {
        return StatusCode((int)HttpStatusCode.BadRequest, CreateResponse(HttpStatusCode.BadRequest, message, data));
    }

    protected IActionResult InternalServerError(string message)
    {
        return StatusCode((int)HttpStatusCode.InternalServerError, CreateResponse(HttpStatusCode.InternalServerError, message));
    }

#nullable disable
}