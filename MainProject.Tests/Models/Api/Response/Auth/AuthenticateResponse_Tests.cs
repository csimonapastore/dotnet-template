using System;
using System.Reflection;
using System.Net;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BasicDotnetTemplate.MainProject;
using BasicDotnetTemplate.MainProject.Models.Api.Response;
using Microsoft.Extensions.DependencyModel.Resolution;
using BasicDotnetTemplate.MainProject.Models.Api.Common.User;
using BasicDotnetTemplate.MainProject.Models.Api.Common.Role;
using DatabaseSqlServer = BasicDotnetTemplate.MainProject.Models.Database.SqlServer;
using BasicDotnetTemplate.MainProject.Models.Api.Response.Auth;


namespace BasicDotnetTemplate.MainProject.Tests;

[TestClass]
public class AuthenticateResponse_Tests
{
    [TestMethod]
    public void IstantiateAuthenticateResponse_OnlyStatus_Valid()
    {
        try
        {
            var authenticateResponse = new AuthenticateResponse(200, null, null);
            Assert.IsTrue(authenticateResponse.Status == 200 && String.IsNullOrEmpty(authenticateResponse.Message) && authenticateResponse.Data == null);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex.Message}");
        }
    }

    [TestMethod]
    public void IstantiateAuthenticateResponse_OnlyStatus_IsInvalid()
    {
        try
        {
            var authenticateResponse = new AuthenticateResponse(201, null, null);
            Assert.IsFalse(authenticateResponse.Status == 200);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex.Message}");
        }
    }

    [TestMethod]
    public void IstantiateAuthenticateResponse_StatusAndMessage_Valid()
    {
        try
        {
            var authenticateResponse = new AuthenticateResponse(200, "This is a test message", null);
            Assert.IsTrue(authenticateResponse.Status == 200 && authenticateResponse.Message == "This is a test message" && authenticateResponse.Data == null);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex.Message}");
        }
    }

    [TestMethod]
    public void IstantiateAuthenticateResponse_AllFields_Valid()
    {
        try
        {
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
            AuthenticatedUser data = new AuthenticatedUser(user);
            var authenticateResponse = new AuthenticateResponse(200, "This is a test message", data);
            Assert.IsTrue(authenticateResponse.Status == 200 && authenticateResponse.Message == "This is a test message" && authenticateResponse.Data == data);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex.Message}");
        }
    }
}




