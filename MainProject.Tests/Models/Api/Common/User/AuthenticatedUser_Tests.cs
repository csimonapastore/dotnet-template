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
public class AuthenticatedUser_Tests
{
    [TestMethod]
    public void InitializeAuthenticatedUserFromUser()
    {
        try
        {
            DatabaseSqlServer.User user = ModelsInit.CreateUser();
            AuthenticatedUser authenticatedUser = new AuthenticatedUser(user);

            Assert.AreEqual(user.FirstName, authenticatedUser.FirstName);
            Assert.AreEqual(user.LastName, authenticatedUser.LastName);
            Assert.AreEqual(user.Email, authenticatedUser.Email);
            Assert.IsInstanceOfType(authenticatedUser.Role, typeof(UserRole));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }
}
