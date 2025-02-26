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
            return;


        }
    }
}
