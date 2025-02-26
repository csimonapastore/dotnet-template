using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BasicDotnetTemplate.MainProject.Core.Database;

namespace BasicDotnetTemplate.MainProject.Services;

public interface IJwtService
{

}

public class JwtService : BaseService, IJwtService
{
    private readonly string _jwtKey;
    private readonly string _jwtIssuer;
    private readonly string _jwtAudience;

    public JwtService(
        IConfiguration configuration,
        SqlServerContext sqlServerContext
    ) : base(configuration, sqlServerContext)
    {
        _jwtKey = _appSettings?.JwtSettings?.Secret ?? String.Empty;
        _jwtIssuer = _appSettings?.JwtSettings?.ValidIssuer ?? String.Empty;
        _jwtAudience = _appSettings?.JwtSettings?.ValidAudience ?? String.Empty;
    }


    public string GenerateToken(string userId, string username)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var expiration = _appSettings?.JwtSettings?.ExpiredAfterMinsOfInactivity ?? 15;

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("userid", userId)
        };

        var token = new JwtSecurityToken(
            _jwtIssuer,
            _jwtAudience,
            claims,
            expires: DateTime.Now.AddMinutes(expiration),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}

