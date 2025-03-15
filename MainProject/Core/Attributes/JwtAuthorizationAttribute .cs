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
using BasicDotnetTemplate.MainProject.Utils;
using BasicDotnetTemplate.MainProject.Models.Api.Common.User;


namespace BasicDotnetTemplate.MainProject.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class JwtAuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        public JwtAuthorizationAttribute(
        )
        {
        }

        public static void Unauthorized(AuthorizationFilterContext context)
        {
            context.Result = new UnauthorizedResult();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // If [AllowAnonymous], skip
            if (context.ActionDescriptor.EndpointMetadata.Any(em => em is AllowAnonymousAttribute))
            {
                return;
            }

            string? userGuidFromToken = null;

            var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var appSettings = new AppSettings();
            configuration.GetSection("AppSettings").Bind(appSettings);
            string? headerAuthorization = context.HttpContext.Request.Headers.Authorization.FirstOrDefault();
            AuthenticatedUser? userContext = context.HttpContext.Items["User"] != null ? (AuthenticatedUser?)context.HttpContext.Items["User"] : null;

            if (userContext == null)
            {
                Unauthorized(context);
            }
            else
            {
                if (!String.IsNullOrEmpty(headerAuthorization))
                {
                    userGuidFromToken = JwtAuthorizationAttribute.ValidateToken(headerAuthorization!, appSettings);
                    if (String.IsNullOrEmpty(userGuidFromToken))
                    {
                        Unauthorized(context);
                    }
                    else
                    {
                        if (userContext!.Guid != userGuidFromToken)
                        {
                            Unauthorized(context);
                        }
                    }
                }
                else
                {
                    Unauthorized(context);
                }
            }


        }

        private static string? ValidateToken(string headerAuthorization, AppSettings appSettings)
        {
            JwtTokenUtils _jwtTokenUtils = new(appSettings);
            return _jwtTokenUtils.ValidateToken(headerAuthorization);
        }
    }
}
