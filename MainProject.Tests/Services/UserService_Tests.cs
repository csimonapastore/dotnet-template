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
public class UserService_Tests
{
    [TestMethod]
    public void Inizialize()
    {
        try
        {
            var userService = TestUtils.CreateUserService();
            if(userService != null)
            {
                Assert.IsInstanceOfType(userService, typeof(UserService));
            }
            else
            {
                Assert.Fail($"UserService is null");
            }            
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex.Message}");
        }
    }

    [TestMethod]
    public async Task GetUserByUsernameAndPassword_Null()
    {
        try
        {
            var userService = TestUtils.CreateUserService();
            var testString = "test";
            if(userService != null)
            {
                var user = await userService.GetUserByUsernameAndPassword(testString, testString);
                Assert.IsTrue(user == null);
            }
            else
            {
                Assert.Fail($"UserService is null");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex.Message}");
        }
    }

    // TODO
    // [TestMethod]
    public async Task GetUserByUsernameAndPassword_Success()
    {
        try
        {
            var userService = TestUtils.CreateUserService();
            var testUsername = "test@email.it";
            var testPassword = "password";
            if(userService != null)
            {
                var user = await userService.GetUserByUsernameAndPassword(testUsername, testPassword);
                Assert.IsTrue(user != null);
                Assert.IsTrue(user.Username == testUsername);
            }
            else
            {
                Assert.Fail($"UserService is null");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex.Message}");
        }
    }


}




