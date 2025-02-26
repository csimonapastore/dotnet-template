using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BasicDotnetTemplate.MainProject.Models.Settings;

namespace BasicDotnetTemplate.MainProject.Services;

public class BaseService
{
    protected readonly IConfiguration _configuration;
    protected readonly AppSettings _appSettings;

    public BaseService(IConfiguration configuration)
    {
        _configuration = configuration;
        _appSettings = new AppSettings();
        _configuration.GetSection("AppSettings").Bind(_appSettings);
    }
}

