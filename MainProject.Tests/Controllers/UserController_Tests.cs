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
using AutoMapper;
using BasicDotnetTemplate.MainProject.Core.Middlewares;
using Microsoft.AspNetCore.Http;


namespace BasicDotnetTemplate.MainProject.Tests;

[TestClass]
public class UserControllerTests
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
    public void UserController_NullConfiguration()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
        var exception = true;
        try
        {
            var userServiceMock = new Mock<IUserService>();
            var roleServiceMock = new Mock<IRoleService>();
            _ = new UserController(null, userServiceMock.Object, roleServiceMock.Object);
            exception = false;
            Assert.Fail($"This test should not pass as configuration is null");
        }
        catch (Exception)
        {
            Assert.IsTrue(exception);
        }
    }


    [TestMethod]
    public async Task GetUserByGuidAsync_Should_Return_200_When_Successful()
    {
        IConfiguration configuration = TestUtils.CreateConfiguration();
        var userServiceMock = new Mock<IUserService>();
        var roleServiceMock = new Mock<IRoleService>();
        var controller = new UserController(configuration, userServiceMock.Object, roleServiceMock.Object);
        var guid = Guid.NewGuid().ToString();
        DatabaseSqlServer.User user = ModelsInit.CreateUser();

        userServiceMock.Setup(s => s.GetUserByGuidAsync(It.IsAny<string>())).ReturnsAsync(user);
        ObjectResult response = (ObjectResult)(await controller.GetUserByGuidAsync(guid));
        if (response != null && response.Value != null)
        {
            Assert.IsTrue(response.StatusCode == StatusCodes.Status200OK);

            var result = (BaseResponse<object>)response.Value;
            if (result != null)
            {
                Assert.IsTrue(result.Status == StatusCodes.Status200OK);
                Assert.IsInstanceOfType(result.Data, typeof(DatabaseSqlServer.User));
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
    public async Task GetUserByGuidAsync_AuthenticateRequestDataNull()
    {
        IConfiguration configuration = TestUtils.CreateConfiguration();
        var userServiceMock = new Mock<IUserService>();
        var roleServiceMock = new Mock<IRoleService>();
        var controller = new UserController(configuration, userServiceMock.Object, roleServiceMock.Object);

        var guid = String.Empty;
        DatabaseSqlServer.User? user = null;

        userServiceMock.Setup(s => s.GetUserByGuidAsync(It.IsAny<string>())).ReturnsAsync(user);
        ObjectResult response = (ObjectResult)(await controller.GetUserByGuidAsync(guid));

        if (response != null && response.Value != null)
        {
            Assert.IsTrue(response.StatusCode == StatusCodes.Status400BadRequest);

            var result = (BaseResponse<object>)response.Value;
            if (result != null)
            {
                Assert.IsTrue(result.Status == StatusCodes.Status400BadRequest);
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

    [TestMethod]
    public async Task GetUserByGuidAsync_NotFound()
    {
        IConfiguration configuration = TestUtils.CreateConfiguration();
        var userServiceMock = new Mock<IUserService>();
        var roleServiceMock = new Mock<IRoleService>();
        var controller = new UserController(configuration, userServiceMock.Object, roleServiceMock.Object);

        var guid = Guid.NewGuid().ToString();
        DatabaseSqlServer.User? user = null;
        userServiceMock.Setup(s => s.GetUserByGuidAsync(It.IsAny<string>())).ReturnsAsync(user);
        NotFoundResult response = (NotFoundResult)(await controller.GetUserByGuidAsync(guid));

        Assert.IsInstanceOfType(response, typeof(NotFoundResult));

        if (response != null)
        {
            Assert.IsTrue(response.StatusCode == StatusCodes.Status404NotFound);
        }
        else
        {
            Assert.Fail($"Response is null");
        }
    }

    [TestMethod]
    public async Task GetUserByGuidAsync_ModelInvalid()
    {
        IConfiguration configuration = TestUtils.CreateConfiguration();
        var userServiceMock = new Mock<IUserService>();
        var roleServiceMock = new Mock<IRoleService>();
        var controller = new UserController(configuration, userServiceMock.Object, roleServiceMock.Object);

        var guid = Guid.NewGuid().ToString();
        DatabaseSqlServer.User? user = null;
        userServiceMock.Setup(s => s.GetUserByGuidAsync(It.IsAny<string>())).ReturnsAsync(user);
        controller.ModelState.AddModelError("Data", "Invalid data");
        ObjectResult response = (ObjectResult)(await controller.GetUserByGuidAsync(guid));

        Assert.IsInstanceOfType(response, typeof(ObjectResult));

        if (response != null && response.Value != null)
        {
            Assert.IsTrue(response.StatusCode == StatusCodes.Status400BadRequest);

            var result = (BaseResponse<object>)response.Value;
            if (result != null)
            {
                Assert.IsTrue(result.Status == StatusCodes.Status400BadRequest);
                Assert.IsTrue(result.Message == "Request is not well formed");
            }
            else
            {
                Assert.Fail($"Result value is null");
            }
        }
        else
        {
            Assert.Fail($"Response is null");
        }
    }

    [TestMethod]
    public async Task GetUserByGuidAsync_Exception()
    {
        IConfiguration configuration = TestUtils.CreateConfiguration();
        var userServiceMock = new Mock<IUserService>();
        var roleServiceMock = new Mock<IRoleService>();
        var controller = new UserController(configuration, userServiceMock.Object, roleServiceMock.Object);

        var guid = Guid.NewGuid().ToString();
        userServiceMock.Setup(s => s.GetUserByGuidAsync(It.IsAny<string>())).ThrowsAsync(new Exception("Unexpected error"));
        ObjectResult response = (ObjectResult)(await controller.GetUserByGuidAsync(guid));

        Assert.IsInstanceOfType(response, typeof(ObjectResult));

        if (response != null && response.Value != null)
        {
            Assert.IsTrue(response.StatusCode == StatusCodes.Status500InternalServerError);

            var result = (BaseResponse<object>)response.Value;
            if (result != null)
            {
                Assert.IsTrue(result.Status == StatusCodes.Status500InternalServerError);
                Assert.IsTrue(result.Message == "Something went wrong. Unexpected error");
            }
            else
            {
                Assert.Fail($"Result value is null");
            }
        }
        else
        {
            Assert.Fail($"Response is null");
        }
    }

}
