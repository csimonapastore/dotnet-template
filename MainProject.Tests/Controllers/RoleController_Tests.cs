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
using BasicDotnetTemplate.MainProject.Controllers;
using BasicDotnetTemplate.MainProject.Services;
using BasicDotnetTemplate.MainProject.Models.Api.Response;
using BasicDotnetTemplate.MainProject.Models.Api.Common.Role;
using DatabaseSqlServer = BasicDotnetTemplate.MainProject.Models.Database.SqlServer;
using Microsoft.AspNetCore.Http;
using BasicDotnetTemplate.MainProject.Models.Api.Request.Role;
using BasicDotnetTemplate.MainProject.Models.Api.Data.Role;
using BasicDotnetTemplate.MainProject.Models.Database.SqlServer;
using Newtonsoft.Json;


namespace BasicDotnetTemplate.MainProject.Tests;

[TestClass]
public class RoleController_Tests
{
    private Mock<IRoleService>? _roleServiceMock;
    private RoleController? _roleController;

    [TestInitialize]
    public void Setup()
    {
        IConfiguration configuration = TestUtils.CreateConfiguration();
        _roleServiceMock = new Mock<IRoleService>();
        _roleController = new RoleController(configuration, _roleServiceMock.Object);
    }

    [TestMethod]
    public void RoleController_NullConfiguration()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
        var exception = true;
        try
        {
            var roleServiceMock = new Mock<IRoleService>();
            _ = new RoleController(null, roleServiceMock.Object);
            exception = false;
            Assert.Fail($"This test should not pass as configuration is null");
        }
        catch (Exception)
        {
            Assert.IsTrue(exception);
        }
    }

    #region "GET"

    [TestMethod]
    public async Task GetRoleByGuidAsync_Should_Return_200_When_Successful()
    {
        if (_roleController == null)
        {
            Assert.Fail($"_roleController is null");
        }
        var guid = Guid.NewGuid().ToString();
        DatabaseSqlServer.Role role = ModelsInit.CreateRole();

        _roleServiceMock?.Setup(s => s.GetRoleByGuidAsync(It.IsAny<string>())).ReturnsAsync(role);
        ObjectResult response = (ObjectResult)(await _roleController.GetRoleByGuidAsync(guid));
        if (response != null && response.Value != null)
        {
            Assert.AreEqual(StatusCodes.Status200OK, response.StatusCode);

            var result = (BaseResponse<object>)response.Value;
            if (result != null)
            {
                Assert.AreEqual(StatusCodes.Status200OK, result.Status);
                Assert.IsInstanceOfType(result.Data, typeof(RoleDto));
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
    public async Task GetRoleByGuidAsync_NotFound()
    {
        if (_roleController == null)
        {
            Assert.Fail($"_roleController is null");
        }

        var guid = Guid.NewGuid().ToString();
        DatabaseSqlServer.Role? role = null;
        _roleServiceMock?.Setup(s => s.GetRoleByGuidAsync(It.IsAny<string>())).ReturnsAsync(role);
        NotFoundResult response = (NotFoundResult)(await _roleController.GetRoleByGuidAsync(guid));

        Assert.IsInstanceOfType(response, typeof(NotFoundResult));

        if (response != null)
        {
            Assert.AreEqual(StatusCodes.Status404NotFound, response.StatusCode);
        }
        else
        {
            Assert.Fail($"Response is null");
        }
    }

    [TestMethod]
    public async Task GetRoleByGuidAsync_Exception()
    {
        if (_roleController == null)
        {
            Assert.Fail($"_roleController is null");
        }

        var guid = Guid.NewGuid().ToString();
        _roleServiceMock?.Setup(s => s.GetRoleByGuidAsync(It.IsAny<string>())).ThrowsAsync(new Exception("Unexpected error"));
        ObjectResult response = (ObjectResult)(await _roleController.GetRoleByGuidAsync(guid));

        Assert.IsInstanceOfType(response, typeof(ObjectResult));

        if (response != null && response.Value != null)
        {
            Assert.AreEqual(StatusCodes.Status500InternalServerError, response.StatusCode);

            var result = (BaseResponse<object>)response.Value;
            if (result != null)
            {
                Assert.AreEqual(StatusCodes.Status500InternalServerError, result.Status);
                Assert.AreEqual("Something went wrong. Unexpected error", result.Message);
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

    #endregion

    #region "CREATE"

    [TestMethod]
    public async Task CreateRoleAsync_Should_Return_200_When_Successful()
    {
        if (_roleController == null)
        {
            Assert.Fail($"_roleController is null");
        }

        if (_roleServiceMock == null)
        {
            Assert.Fail($"_roleServiceMock is null");
        }

        DatabaseSqlServer.Role role = ModelsInit.CreateRole();

        CreateRoleRequest request = new CreateRoleRequest()
        {
            Data = new CreateRoleRequestData()
            {
                Name = "RoleTest",
                IsNotEditable = true
            }
        };

        _roleServiceMock?.Setup(s => s.CheckIfNameIsValid(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
        _roleServiceMock?.Setup(s => s.CreateRoleAsync(request.Data)).ReturnsAsync(role);

        ObjectResult response = (ObjectResult)(await _roleController.CreateRoleAsync(request));
        if (response != null && response.Value != null)
        {
            Assert.AreEqual(StatusCodes.Status200OK, response.StatusCode);

            var result = (BaseResponse<object>)response.Value;
            if (result != null)
            {
                Assert.AreEqual(StatusCodes.Status200OK, result.Status);
                Assert.IsInstanceOfType(result.Data, typeof(RoleDto));
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
    public async Task CreateRoleAsync_InvalidName()
    {
        if (_roleController == null)
        {
            Assert.Fail($"_roleController is null");
        }

        CreateRoleRequest request = new CreateRoleRequest()
        {
            Data = new CreateRoleRequestData()
            {
                Name = "RoleTest",
                IsNotEditable = true
            }
        };

        _roleServiceMock?.Setup(s => s.CheckIfNameIsValid(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

        ObjectResult response = (ObjectResult)(await _roleController.CreateRoleAsync(request));

        if (response != null && response.Value != null)
        {
            Assert.AreEqual(StatusCodes.Status400BadRequest, response.StatusCode);

            var result = (BaseResponse<object>)response.Value;
            if (result != null)
            {
                Assert.AreEqual(StatusCodes.Status400BadRequest, result.Status);
                Assert.AreEqual("Invalid name", result.Message);
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
    public async Task CreateRoleAsync_NotCreated()
    {
        if (_roleController == null)
        {
            Assert.Fail($"_roleController is null");
        }

        if (_roleServiceMock == null)
        {
            Assert.Fail($"_roleServiceMock is null");
        }

        DatabaseSqlServer.Role role = ModelsInit.CreateRole();
        DatabaseSqlServer.Role? expectedRole = null;

        CreateRoleRequest request = new CreateRoleRequest()
        {
            Data = new CreateRoleRequestData()
            {
                Name = "RoleTest",
                IsNotEditable = true
            }
        };

        _roleServiceMock?.Setup(s => s.CheckIfNameIsValid(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
        _roleServiceMock?.Setup(s => s.CreateRoleAsync(request.Data)).ReturnsAsync(expectedRole);

        ObjectResult response = (ObjectResult)(await _roleController.CreateRoleAsync(request));
        if (response != null && response.Value != null)
        {
            Assert.AreEqual(StatusCodes.Status400BadRequest, response.StatusCode);

            var result = (BaseResponse<object>)response.Value;
            if (result != null)
            {
                Assert.AreEqual(StatusCodes.Status400BadRequest, result.Status);
                Assert.AreEqual("Not created", result.Message);
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
    public async Task CreateRoleAsync_Exception()
    {
        if (_roleController == null)
        {
            Assert.Fail($"_roleController is null");
        }

        if (_roleServiceMock == null)
        {
            Assert.Fail($"_roleServiceMock is null");
        }

        DatabaseSqlServer.Role role = ModelsInit.CreateRole();

        CreateRoleRequest request = new CreateRoleRequest()
        {
            Data = new CreateRoleRequestData()
            {
                Name = "RoleTest",
                IsNotEditable = true
            }
        };

        _roleServiceMock?.Setup(s => s.CheckIfNameIsValid(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
        _roleServiceMock?.Setup(s => s.CreateRoleAsync(
            It.IsAny<CreateRoleRequestData>()
        )).ThrowsAsync(new Exception("Unexpected error"));

        ObjectResult response = (ObjectResult)(await _roleController.CreateRoleAsync(request));
        Assert.IsInstanceOfType(response, typeof(ObjectResult));

        if (response != null && response.Value != null)
        {
            Assert.AreEqual(StatusCodes.Status500InternalServerError, response.StatusCode);

            var result = (BaseResponse<object>)response.Value;
            if (result != null)
            {
                Assert.AreEqual(StatusCodes.Status500InternalServerError, result.Status);
                Assert.AreEqual("Something went wrong. Unexpected error", result.Message);
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

    #endregion

    #region "DELETE"

    [TestMethod]
    public async Task DeleteRoleByGuidAsync_Success()
    {
        if (_roleController == null)
        {
            Assert.Fail($"_roleController is null");
        }
        var guid = Guid.NewGuid().ToString();
        DatabaseSqlServer.Role role = ModelsInit.CreateRole();

        _roleServiceMock?.Setup(s => s.GetRoleByGuidAsync(It.IsAny<string>())).ReturnsAsync(role);
        ObjectResult response = (ObjectResult)(await _roleController.DeleteRoleByGuidAsync(guid));
        if (response != null && response.Value != null)
        {
            Assert.AreEqual(StatusCodes.Status200OK, response.StatusCode);
        }
        else
        {
            Assert.Fail($"Response value is null");
        }
    }

    [TestMethod]
    public async Task DeleteRoleByGuidAsync_NotFound()
    {
        if (_roleController == null)
        {
            Assert.Fail($"_roleController is null");
        }

        var guid = Guid.NewGuid().ToString();
        DatabaseSqlServer.Role? role = null;
        _roleServiceMock?.Setup(s => s.GetRoleByGuidAsync(It.IsAny<string>())).ReturnsAsync(role);
        NotFoundResult response = (NotFoundResult)(await _roleController.DeleteRoleByGuidAsync(guid));

        Assert.IsInstanceOfType(response, typeof(NotFoundResult));

        if (response != null)
        {
            Assert.AreEqual(StatusCodes.Status404NotFound, response.StatusCode);
        }
        else
        {
            Assert.Fail($"Response is null");
        }
    }

    [TestMethod]
    public async Task DeleteRoleByGuidAsync_Exception()
    {
        if (_roleController == null)
        {
            Assert.Fail($"_roleController is null");
        }

        var guid = Guid.NewGuid().ToString();
        _roleServiceMock?.Setup(s => s.GetRoleByGuidAsync(It.IsAny<string>())).ThrowsAsync(new Exception("Unexpected error"));
        ObjectResult response = (ObjectResult)(await _roleController.DeleteRoleByGuidAsync(guid));

        Assert.IsInstanceOfType(response, typeof(ObjectResult));

        if (response != null && response.Value != null)
        {
            Assert.AreEqual(StatusCodes.Status500InternalServerError, response.StatusCode);

            var result = (BaseResponse<object>)response.Value;
            if (result != null)
            {
                Assert.AreEqual(StatusCodes.Status500InternalServerError, result.Status);
                Assert.AreEqual("Something went wrong. Unexpected error", result.Message);
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

    #endregion



    #region "UPDATE"

    [TestMethod]
    public async Task UpdateRoleAsync_Should_Return_200_When_Successful()
    {
        if (_roleController == null)
        {
            Assert.Fail($"_roleController is null");
        }

        if (_roleServiceMock == null)
        {
            Assert.Fail($"_roleServiceMock is null");
        }

        DatabaseSqlServer.Role role = ModelsInit.CreateRole();

        CreateRoleRequest request = new CreateRoleRequest()
        {
            Data = new CreateRoleRequestData()
            {
                Name = "RoleTest",
                IsNotEditable = true
            }
        };

        _roleServiceMock?.Setup(s => s.GetRoleByGuidAsync(It.IsAny<string>())).ReturnsAsync(role);
        _roleServiceMock?.Setup(s => s.CheckIfNameIsValid(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
        _roleServiceMock?.Setup(s => s.UpdateRoleAsync(It.IsAny<CreateRoleRequestData>(), It.IsAny<Role>())).ReturnsAsync(role);

        ObjectResult response = (ObjectResult)(await _roleController.UpdateRoleAsync(request, role.Guid));
        if (response != null && response.Value != null)
        {
            Assert.AreEqual(StatusCodes.Status200OK, response.StatusCode);

            var result = (BaseResponse<object>)response.Value;
            if (result != null)
            {
                Assert.AreEqual(StatusCodes.Status200OK, result.Status);
                Assert.IsInstanceOfType(result.Data, typeof(RoleDto));
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
    public async Task UpdateRoleAsync_RoleNotFound()
    {
        if (_roleController == null)
        {
            Assert.Fail($"_roleController is null");
        }

        DatabaseSqlServer.Role? role = null;

        CreateRoleRequest request = new CreateRoleRequest()
        {
            Data = new CreateRoleRequestData()
            {
                Name = "RoleTest",
                IsNotEditable = true
            }
        };

        _roleServiceMock?.Setup(s => s.GetRoleByGuidAsync(It.IsAny<string>())).ReturnsAsync(role);

        NotFoundResult response = (NotFoundResult)(await _roleController.UpdateRoleAsync(request, Guid.NewGuid().ToString()));

        if (response != null)
        {
            Assert.AreEqual(StatusCodes.Status404NotFound, response.StatusCode);
        }
        else
        {
            Assert.Fail($"Response is null");
        }
    }

    [TestMethod]
    public async Task UpdateRoleAsync_InvalidName()
    {
        if (_roleController == null)
        {
            Assert.Fail($"_roleController is null");
        }

        DatabaseSqlServer.Role role = ModelsInit.CreateRole();

        CreateRoleRequest request = new CreateRoleRequest()
        {
            Data = new CreateRoleRequestData()
            {
                Name = "RoleTest",
                IsNotEditable = true
            }
        };

        _roleServiceMock?.Setup(s => s.GetRoleByGuidAsync(It.IsAny<string>())).ReturnsAsync(role);
        _roleServiceMock?.Setup(s => s.CheckIfNameIsValid(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

        ObjectResult response = (ObjectResult)(await _roleController.UpdateRoleAsync(request, role.Guid));

        if (response != null && response.Value != null)
        {
            Assert.AreEqual(StatusCodes.Status400BadRequest, response.StatusCode);

            var result = (BaseResponse<object>)response.Value;
            if (result != null)
            {
                Assert.AreEqual(StatusCodes.Status400BadRequest, result.Status);
                Assert.AreEqual("Invalid name", result.Message);
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
    public async Task UpdateRoleAsync_NotEditable()
    {
        if (_roleController == null)
        {
            Assert.Fail($"_roleController is null");
        }

        DatabaseSqlServer.Role role = ModelsInit.CreateRole();
        role.IsNotEditable = true;

        CreateRoleRequest request = new CreateRoleRequest()
        {
            Data = new CreateRoleRequestData()
            {
                Name = "RoleTest",
                IsNotEditable = true
            }
        };

        _roleServiceMock?.Setup(s => s.GetRoleByGuidAsync(It.IsAny<string>())).ReturnsAsync(role);
        _roleServiceMock?.Setup(s => s.CheckIfNameIsValid(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

        ObjectResult response = (ObjectResult)(await _roleController.UpdateRoleAsync(request, role.Guid));

        if (response != null && response.Value != null)
        {
            Assert.AreEqual(StatusCodes.Status400BadRequest, response.StatusCode);

            var result = (BaseResponse<object>)response.Value;
            if (result != null)
            {
                Assert.AreEqual(StatusCodes.Status400BadRequest, result.Status);
                Assert.AreEqual("This role is not editable", result.Message);
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
    public async Task UpdateRoleAsync_Exception()
    {
        if (_roleController == null)
        {
            Assert.Fail($"_roleController is null");
        }

        if (_roleServiceMock == null)
        {
            Assert.Fail($"_roleServiceMock is null");
        }

        DatabaseSqlServer.Role role = ModelsInit.CreateRole();

        CreateRoleRequest request = new CreateRoleRequest()
        {
            Data = new CreateRoleRequestData()
            {
                Name = "RoleTest",
                IsNotEditable = true
            }
        };

        _roleServiceMock?.Setup(s => s.GetRoleByGuidAsync(It.IsAny<string>())).ReturnsAsync(role);
        _roleServiceMock?.Setup(s => s.CheckIfNameIsValid(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
        _roleServiceMock?.Setup(s => s.UpdateRoleAsync(
            It.IsAny<CreateRoleRequestData>(), It.IsAny<Role>()
        )).ThrowsAsync(new Exception("Unexpected error"));

        ObjectResult response = (ObjectResult)(await _roleController.UpdateRoleAsync(request, role.Guid));
        Assert.IsInstanceOfType(response, typeof(ObjectResult));

        if (response != null && response.Value != null)
        {
            Assert.AreEqual(StatusCodes.Status500InternalServerError, response.StatusCode);

            var result = (BaseResponse<object>)response.Value;
            if (result != null)
            {
                Assert.AreEqual(StatusCodes.Status500InternalServerError, result.Status);
                Assert.AreEqual("Something went wrong. Unexpected error", result.Message);
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

    #endregion
}
