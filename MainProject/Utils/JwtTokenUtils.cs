using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using BasicDotnetTemplate.MainProject.Models.Settings;
using DatabaseSqlServer = BasicDotnetTemplate.MainProject.Models.Database.SqlServer;

namespace BasicDotnetTemplate.MainProject.Utils;

public class JwtTokenUtils
{
    private readonly string _jwtKey;
    private readonly string _jwtIssuer;
    private readonly string _jwtAudience;
    private readonly int _expiration;
    private readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

    public JwtTokenUtils(AppSettings appSettings)
    {
        _jwtKey = appSettings?.JwtSettings?.Secret ?? String.Empty;
        _jwtIssuer = appSettings?.JwtSettings?.ValidIssuer ?? String.Empty;
        _jwtAudience = appSettings?.JwtSettings?.ValidAudience ?? String.Empty;
        _expiration = appSettings?.JwtSettings?.ExpiredAfterMinsOfInactivity ?? 15;
    }

    public string GenerateToken(string guid)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, guid),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("guid", guid)
        };

        var token = new JwtSecurityToken(
            _jwtIssuer,
            _jwtAudience,
            claims,
            expires: DateTime.Now.AddMinutes(_expiration),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string? ValidateToken(string headerAuthorization)
    {
        string? token = null;
        string? guid = null;

        if (
            String.IsNullOrEmpty(_jwtKey) ||
            String.IsNullOrEmpty(_jwtIssuer) ||
            String.IsNullOrEmpty(_jwtAudience)
        )
        {
            return guid;
        }

        string[] authorizations = headerAuthorization.Split(" ");
        if (authorizations.Length == 2)
        {
            token = authorizations[1];
        }

        if (!String.IsNullOrEmpty(token))
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtKey);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _jwtIssuer,
                    ValidateAudience = true,
                    ValidAudience = _jwtAudience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                if (jwtToken != null)
                {
                    var claimedUserId = jwtToken.Claims.FirstOrDefault(c => c.Type == "guid");
                    if (claimedUserId != null && !String.IsNullOrEmpty(claimedUserId.Value))
                    {
                        guid = claimedUserId.Value;
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception, $"[JwtTokenUtils][ValidateToken]");
                return guid;
            }
        }
        return guid;
    }

}

