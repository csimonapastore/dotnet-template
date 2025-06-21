using BasicDotnetTemplate.MainProject.Models.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using BasicDotnetTemplate.MainProject.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using BasicDotnetTemplate.MainProject.Core.Database;
using BasicDotnetTemplate.MainProject.Services;
using BasicDotnetTemplate.MainProject.Models.Api.Response;
using BasicDotnetTemplate.MainProject.Models.Api.Request.Auth;
using BasicDotnetTemplate.MainProject.Models.Api.Data.Auth;
using BasicDotnetTemplate.MainProject.Models.Api.Common.User;
using BasicDotnetTemplate.MainProject.Models.Api.Common.Role;
using BasicDotnetTemplate.MainProject.Models.Api.Response.Auth;
using DatabaseSqlServer = BasicDotnetTemplate.MainProject.Models.Database.SqlServer;



namespace BasicDotnetTemplate.MainProject.Tests;

[TestClass]
public class AuthService_Tests
{
    [TestMethod]
    public void Inizialize()
    {
        try
        {
            var authService = TestUtils.CreateAuthService();
            if (authService != null)
            {
                Assert.IsInstanceOfType(authService, typeof(AuthService));
            }
            else
            {
                Assert.Fail($"AuthService is null");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task AuthenticateAsync_UserNotFound()
    {
        try
        {
            var request = new AuthenticateRequest
            {
                Data = new AuthenticateRequestData
                {
                    Email = "d2ejdI1f4GYpq2kTB1nmeQkZXqR3QSxH8Yqkl7iv7zgfQ13qG/0dUUsreG/WGHWRBE5mVWaV43A=",
                    Password = "d2ejdI1f4GYpq2kTB1nmeQkZXqR3QSxH8Yqkl7iv7zgfQ13qG/0dUUsreG/WGHWRBE5mVWaV43A="
                }
            };
            var authService = TestUtils.CreateAuthService();
            if (authService != null)
            {
                var authenticatedUser = await authService.AuthenticateAsync(request.Data);
                Assert.IsNull(authenticatedUser);
            }
            else
            {
                Assert.Fail($"AuthService is null");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task AuthenticateAsync_UsernamePasswordInvalid()
    {
        try
        {
            var request = new AuthenticateRequest
            {
                Data = new AuthenticateRequestData
                {
                    Email = "WGHWRBE5mVWaV=",
                    Password = "WGHWRBE5mVWaV="
                }
            };
            var authService = TestUtils.CreateAuthService();
            if (authService != null)
            {
                var authenticatedUser = await authService.AuthenticateAsync(request.Data);
                Assert.IsNull(authenticatedUser);
            }
            else
            {
                Assert.Fail($"AuthService is null");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

}




