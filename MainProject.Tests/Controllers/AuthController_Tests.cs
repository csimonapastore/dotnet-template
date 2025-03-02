using System;
using System.Reflection;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using BasicDotnetTemplate.MainProject;
using BasicDotnetTemplate.MainProject.Controllers;
using BasicDotnetTemplate.MainProject.Services;
using BasicDotnetTemplate.MainProject.Models.Api.Response;
using BasicDotnetTemplate.MainProject.Models.Settings;
using BasicDotnetTemplate.MainProject.Models.Api.Request.Auth;
using BasicDotnetTemplate.MainProject.Models.Api.Data.Auth;
using BasicDotnetTemplate.MainProject.Models.Api.Common.User;
using BasicDotnetTemplate.MainProject.Models.Api.Common.Role;
using DatabaseSqlServer = BasicDotnetTemplate.MainProject.Models.Database.SqlServer;
using BasicDotnetTemplate.MainProject.Models.Api.Response.Auth;


namespace BasicDotnetTemplate.MainProject.Tests;

[TestClass]
public class AuthController_Tests
{
    [TestMethod]
    public void AuthController_NullConfiguration()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
        var exception = true;
        try
        {
            var authServiceMock = new Mock<IAuthService>();
            _ = new AuthController(null, authServiceMock.Object);
            exception = false;
            Assert.Fail($"This test should not pass as configuration is null");
        }
        catch (Exception)
        {
            Assert.IsTrue(exception);
        }
    }


    [TestMethod]
    public async Task AuthenticateAsync_Should_Return_200_When_Successful()
    {
        IConfiguration configuration = TestUtils.CreateConfiguration();
        var authServiceMock = new Mock<IAuthService>();
        var controller = new AuthController(configuration, authServiceMock.Object);
        DatabaseSqlServer.User user = new DatabaseSqlServer.User()
            {
                Username = "test",
                FirstName = "test",
                LastName = "test",
                Email = "test",
                PasswordHash = "test",
                Role = new DatabaseSqlServer.Role()
                {
                    Name = "test"
                }
            };
        AuthenticatedUser authenticatedUser = new AuthenticatedUser(user);

        var request = new AuthenticateRequest { Data = new AuthenticateRequestData { Username = "user", Password = "pass" } };
        authServiceMock.Setup(s => s.AuthenticateAsync(It.IsAny<AuthenticateRequestData>())).ReturnsAsync(authenticatedUser);        
        ObjectResult response = (ObjectResult)(await controller.AuthenticateAsync(request));
        if(response != null && response.Value != null)
        {
            Assert.IsTrue(response.StatusCode == 200);

            var result = (BaseResponse)response.Value;
            if(result != null)
            {
                Assert.IsTrue(result.Status == 200);
                Assert.IsInstanceOfType(result.Data, typeof(AuthenticatedUser));
            }
            else
            {
                Assert.Fail($"Result value is null");
            }
            
        }
        else
        {
            Assert.Fail($"Response value is null");
        }
    }

    [TestMethod]
    public async Task AuthenticateAsync_AuthenticateRequestDataNull()
    {
        IConfiguration configuration = TestUtils.CreateConfiguration();
        var authServiceMock = new Mock<IAuthService>();
        var controller = new AuthController(configuration, authServiceMock.Object);

        var request = new AuthenticateRequest
        {
            Data = null
        };
        AuthenticatedUser authenticatedUser = null;
        authServiceMock.Setup(s => s.AuthenticateAsync(It.IsAny<AuthenticateRequestData>())).ReturnsAsync(authenticatedUser);        
        ObjectResult response = (ObjectResult)(await controller.AuthenticateAsync(request));

        if(response != null && response.Value != null)
        {
            Assert.IsTrue(response.StatusCode == 400);

            var result = (BaseResponse)response.Value;
            if(result != null)
            {
                Assert.IsTrue(result.Status == 400);
                Assert.IsTrue(result.Message == "Request is not well formed");
            }
            else
            {
                Assert.Fail($"Result value is null");
            }            
        }
        else
        {
            Assert.Fail($"Response value is null");
        }
    }

}
