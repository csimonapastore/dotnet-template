using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
        IConfiguration configuration
    ) : base(configuration)
    {
        _jwtKey = _appSettings?.JWTSettings?.Secret ?? String.Empty;
        _jwtIssuer = _appSettings?.JWTSettings?.ValidIssuer ?? String.Empty;
        _jwtAudience = _appSettings?.JWTSettings?.ValidAudience ?? String.Empty;
    }

    
}

