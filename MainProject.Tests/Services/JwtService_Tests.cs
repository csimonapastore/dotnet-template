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
using BasicDotnetTemplate.MainProject.Models.Api.Response.Auth;
using DatabaseSqlServer = BasicDotnetTemplate.MainProject.Models.Database.SqlServer;



namespace BasicDotnetTemplate.MainProject.Tests;

[TestClass]
public class JwtService_Tests
{
    [TestMethod]
    public void Inizialize()
    {
        try
        {
            var jwtService = TestUtils.CreateJwtService();
            if (jwtService != null)
            {
                Assert.IsInstanceOfType(jwtService, typeof(JwtService));
            }
            else
            {
                Assert.Fail($"JwtService is null");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public void GenerateToken()
    {
        try
        {
            var jwtService = TestUtils.CreateJwtService();
            var testString = "test";
            if (jwtService != null)
            {
                var jwt = jwtService.GenerateToken(testString);
                Assert.IsNotNull(jwt);
                Assert.IsInstanceOfType(jwt, typeof(string));
            }
            else
            {
                Assert.Fail($"JwtService is null");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }


}




