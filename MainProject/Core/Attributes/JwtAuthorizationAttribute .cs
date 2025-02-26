using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using BasicDotnetTemplate.MainProject.Models.Settings;

namespace BasicDotnetTemplate.MainProject.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class JwtAuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string? _policyName;

        public JwtAuthorizationAttribute() { }
        public JwtAuthorizationAttribute(string policyName)
        {
            _policyName = policyName;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // If [AllowAnonymous], skip
            if (context.ActionDescriptor.EndpointMetadata.Any(em => em is AllowAnonymousAttribute))
            {
                return;
            }

            var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var appSettings = new AppSettings();
            configuration.GetSection("AppSettings").Bind(appSettings);
            var jwtKey = appSettings.JwtSettings?.Secret ?? String.Empty;
            var jwtIssuer = appSettings.JwtSettings?.ValidIssuer ?? String.Empty;
            var jwtAudience = appSettings.JwtSettings?.ValidAudience ?? String.Empty;
            string token = null;

            if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            string[] authorizations = context.HttpContext.Request.Headers.Authorization.FirstOrDefault()?.Split(" ");
            if(authorizations.Length == 2)
            {
                token = authorizations[1];
            }

            if (token == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(jwtKey);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = jwtIssuer,
                    ValidateAudience = true,
                    ValidAudience = jwtAudience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                if (_policyName != null)
                {
                    var claim = jwtToken.Claims.FirstOrDefault(c => c.Type == _policyName);
                    if (claim == null)
                    {
                        context.Result = new ForbidResult();
                        return;
                    }
                }

            }
            catch
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
