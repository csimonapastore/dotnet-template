using System;
using System.Reflection;
using System.Net;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BasicDotnetTemplate.MainProject;
using BasicDotnetTemplate.MainProject.Models.Api.Response;
using Microsoft.Extensions.DependencyModel.Resolution;
using BasicDotnetTemplate.MainProject.Models.Api.Common.User;
using BasicDotnetTemplate.MainProject.Models.Api.Response.User;
using DatabaseSqlServer = BasicDotnetTemplate.MainProject.Models.Database.SqlServer;
using BasicDotnetTemplate.MainProject.Models.Api.Response.Auth;
using BasicDotnetTemplate.MainProject.Core.Middlewares;
using AutoMapper;


namespace BasicDotnetTemplate.MainProject.Tests;

[TestClass]
public class GetUserResponse_Tests
{
    private IMapper? _mapper;

    [TestInitialize]
    public void Setup()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AutoMapperConfiguration>();
        });

        _mapper = config.CreateMapper();
    }

    [TestMethod]
    public void IstantiateGetUserResponse_OnlyStatus_Valid()
    {
        try
        {
            var getUserResponse = new GetUserResponse(200, null, null);
            Assert.IsTrue(getUserResponse.Status == 200 && String.IsNullOrEmpty(getUserResponse.Message) && getUserResponse.Data == null);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex.Message}");
        }
    }

    [TestMethod]
    public void IstantiateGetUserResponse_OnlyStatus_IsInvalid()
    {
        try
        {
            var getUserResponse = new GetUserResponse(201, null, null);
            Assert.IsFalse(getUserResponse.Status == 200);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex.Message}");
        }
    }

    [TestMethod]
    public void IstantiateGetUserResponse_StatusAndMessage_Valid()
    {
        try
        {
            var getUserResponse = new GetUserResponse(200, "This is a test message", null);
            Assert.IsTrue(getUserResponse.Status == 200 && getUserResponse.Message == "This is a test message" && getUserResponse.Data == null);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex.Message}");
        }
    }

    [TestMethod]
    public void IstantiateGetUserResponse_AllFields_Valid()
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
                PasswordSalt = "test",
                Password = "test",
                Role = new DatabaseSqlServer.Role()
                {
                    Name = "test"
                },
                IsTestUser = true
            };
            UserDto? data = _mapper?.Map<UserDto>(user);
            var getUserResponse = new GetUserResponse(200, "This is a test message", data);
            Assert.IsTrue(getUserResponse.Status == 200 && getUserResponse.Message == "This is a test message" && getUserResponse.Data == data);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex.Message}");
        }
    }

}




