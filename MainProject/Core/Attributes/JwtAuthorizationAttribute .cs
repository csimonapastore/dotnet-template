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
using BasicDotnetTemplate.MainProject.Services;
using DatabaseSqlServer = BasicDotnetTemplate.MainProject.Models.Database.SqlServer;


namespace BasicDotnetTemplate.MainProject.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class JwtAuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        private readonly IJwtService _jwtService;

        public JwtAuthorizationAttribute(
            IJwtService jwtService
        )
        {
            _jwtService = jwtService;
        }

        public static void Unauthorized(AuthorizationFilterContext context)
        {
            context.Result = new UnauthorizedResult();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            DatabaseSqlServer.User? user = null;
            // If [AllowAnonymous], skip
            if (context.ActionDescriptor.EndpointMetadata.Any(em => em is AllowAnonymousAttribute))
            {
                return;
            }

            var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var appSettings = new AppSettings();
            configuration.GetSection("AppSettings").Bind(appSettings);
            string? headerAuthorization = context.HttpContext.Request.Headers.Authorization.FirstOrDefault();
            
            if(!String.IsNullOrEmpty(headerAuthorization))
            {
                user = _jwtService.ValidateToken(headerAuthorization!);
                if(user == null)
                {
                    Unauthorized(context);
                }
            }
            else
            {
                Unauthorized(context);
            }
        }
    }
}
