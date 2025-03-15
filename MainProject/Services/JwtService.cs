using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BasicDotnetTemplate.MainProject.Core.Database;
using BasicDotnetTemplate.MainProject.Utils;
using DatabaseSqlServer = BasicDotnetTemplate.MainProject.Models.Database.SqlServer;


namespace BasicDotnetTemplate.MainProject.Services;

public interface IJwtService
{
    string GenerateToken(string guid);
}

public class JwtService : BaseService, IJwtService
{
    private readonly JwtTokenUtils _jwtTokenUtils;
    private readonly IUserService _userService;

    public JwtService(
        IConfiguration configuration,
        SqlServerContext sqlServerContext,
        IUserService userService
    ) : base(configuration, sqlServerContext)
    {
        _jwtTokenUtils = new JwtTokenUtils(_appSettings);
        _userService = userService;
    }


    public string GenerateToken(string guid)
    {
        return _jwtTokenUtils.GenerateToken(guid);
    }

    

}

