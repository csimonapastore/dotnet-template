using System.Net;
using Microsoft.AspNetCore.Mvc;
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




}