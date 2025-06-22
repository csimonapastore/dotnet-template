using BasicDotnetTemplate.MainProject.Services;
using BasicDotnetTemplate.MainProject.Models.Api.Data.User;
using BasicDotnetTemplate.MainProject.Models.Database.SqlServer;
using Newtonsoft.Json;
using BasicDotnetTemplate.MainProject.Models.Api.Common.Exceptions;
using Microsoft.AspNetCore.Mvc.Diagnostics;


namespace BasicDotnetTemplate.MainProject.Tests;

[TestClass]
public class PermissionService_Tests
{
    private static PermissionService _permissionService = TestUtils.CreatePermissionService();
    private static string _name = "TEST";
    private static PermissionSystem _permissionSystem = new PermissionSystem()
    {
        Name = _name + "-SYSTEM",
        Enabled = false
    };

    private static PermissionModule _permissionModule = new PermissionModule()
    {
        Name = _name + "-MODULE",
        Enabled = false
    };

    private static PermissionOperation _permissionOperation = new PermissionOperation()
    {
        Name = _name + "-OPERATION"
    };

    private static PermissionSystemModule _permissionSystemModule = new PermissionSystemModule()
    {
        PermissionSystem = _permissionSystem,
        PermissionSystemId = 0,
        PermissionModule = _permissionModule,
        PermissionModuleId = 0,
        Enabled = false
    };

    private static PermissionSystemModuleOperation _permissionSystemModuleOperation = new PermissionSystemModuleOperation()
    {
        PermissionSystemModule = _permissionSystemModule,
        PermissionSystemModuleId = 0,
        PermissionOperation = _permissionOperation,
        PermissionOperationId = 0,
        Enabled = false
    };

    private static RolePermissionSystemModuleOperation _rolePermissionSystemModuleOperation = new RolePermissionSystemModuleOperation()
    {
        PermissionSystemModuleOperation = _permissionSystemModuleOperation,
        PermissionSystemModuleOperationId = 0,
        Role = new Role()
        {
            Name = _name + "-ROLE",
            IsNotEditable = false
        },
        RoleId = 0,
        Active = false
    };

    [TestMethod]
    public void Inizialize()
    {
        try
        {
            var permissionService = TestUtils.CreatePermissionService();
            if (permissionService != null)
            {
                Assert.IsInstanceOfType(permissionService, typeof(PermissionService));
            }
            else
            {
                Assert.Fail($"PermissionService is null");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex.Message}");
        }
    }

    #region "PermissionSystem"

    [TestMethod]
    public async Task GetPermissionSystemByGuidAsync_Null()
    {
        try
        {

            if (_permissionService != null)
            {
                var permission = await _permissionService.GetPermissionSystemByGuidAsync(Guid.NewGuid().ToString());
                Assert.IsNull(permission);
            }
            else
            {
                Assert.Fail($"PermissionService is null");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task GetPermissionSystemByNameAsync_Null()
    {
        try
        {

            if (_permissionService != null)
            {
                var permission = await _permissionService.GetPermissionSystemByNameAsync(Guid.NewGuid().ToString());
                Assert.IsNull(permission);
            }
            else
            {
                Assert.Fail($"PermissionService is null");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task CreatePermissionSystemAsync_Success()
    {
        try
        {
            var permission = await _permissionService.CreatePermissionSystemAsync(_permissionSystem.Name, true);
            Assert.IsInstanceOfType(permission, typeof(PermissionSystem));
            Assert.IsNotNull(permission);
            Assert.AreEqual(_permissionSystem.Name, permission.Name);
            Assert.IsTrue(permission.Enabled);
            _permissionSystem = permission;

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task CreatePermissionSystemAsync_Exception()
    {
        try
        {
            var exceptionPermissionService = TestUtils.CreatePermissionServiceException();

            if (exceptionPermissionService != null)
            {
                try
                {
                    var permission = await exceptionPermissionService.CreatePermissionSystemAsync(_permissionSystem.Name, true);
                    Assert.Fail($"Expected exception instead of response: {permission?.Guid}");
                }
                catch (Exception exception)
                {
                    Assert.IsInstanceOfType(exception, typeof(Exception));
                    Assert.IsInstanceOfType(exception, typeof(CreateException));
                }
            }
            else
            {
                Assert.Fail($"PermissionService is null");
            }
        }
        catch (Exception ex)
        {
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task HandleEnabledPermissionSystemAsync()
    {
        try
        {
            var updated = await _permissionService.HandleEnabledPermissionSystemAsync(_permissionSystem, false);
            Assert.IsTrue(updated);
            Assert.IsFalse(_permissionSystem.Enabled);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task GetPermissionSystemByGuidAsync_Success()
    {
        try
        {

            if (_permissionService != null)
            {
                var permission = await _permissionService.GetPermissionSystemByGuidAsync(_permissionSystem.Guid);
                Assert.IsNotNull(permission);
                Assert.IsInstanceOfType(permission, typeof(PermissionSystem));
                Assert.AreEqual(_permissionSystem.Name, permission.Name);
                Assert.AreEqual( _permissionSystem.Enabled, permission.Enabled);
            }
            else
            {
                Assert.Fail($"PermissionService is null");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task GetPermissionSystemByNameAsync_Success()
    {
        try
        {

            if (_permissionService != null)
            {
                var permission = await _permissionService.GetPermissionSystemByNameAsync(_permissionSystem.Name);
                Assert.IsNotNull(permission);
                Assert.IsInstanceOfType(permission, typeof(PermissionSystem));
                Assert.AreEqual(_permissionSystem.Guid, permission.Guid);
                Assert.AreEqual( _permissionSystem.Enabled, permission.Enabled);
            }
            else
            {
                Assert.Fail($"PermissionService is null");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    #endregion


    #region "PermissionModule"

    [TestMethod]
    public async Task GetPermissionModuleByGuidAsync_Null()
    {
        try
        {

            if (_permissionService != null)
            {
                var permission = await _permissionService.GetPermissionModuleByGuidAsync(Guid.NewGuid().ToString());
                Assert.IsNull(permission);
            }
            else
            {
                Assert.Fail($"PermissionService is null");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task GetPermissionModuleByNameAsync_Null()
    {
        try
        {

            if (_permissionService != null)
            {
                var permission = await _permissionService.GetPermissionModuleByNameAsync(Guid.NewGuid().ToString());
                Assert.IsNull(permission);
            }
            else
            {
                Assert.Fail($"PermissionService is null");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task CreatePermissionModuleAsync_Success()
    {
        try
        {
            var permission = await _permissionService.CreatePermissionModuleAsync(_permissionModule.Name, true);
            Assert.IsInstanceOfType(permission, typeof(PermissionModule));
            Assert.IsNotNull(permission);
            Assert.AreEqual(_permissionModule.Name, permission.Name);
            Assert.IsTrue(permission.Enabled);
            _permissionModule = permission;

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task CreatePermissionModuleAsync_Exception()
    {
        try
        {

            var exceptionPermissionService = TestUtils.CreatePermissionServiceException();

            if (exceptionPermissionService != null)
            {
                try
                {
                    var permission = await exceptionPermissionService.CreatePermissionModuleAsync(_permissionModule.Name, true);
                    Assert.Fail($"Expected exception instead of response: {permission?.Guid}");
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    Assert.IsInstanceOfType(exception, typeof(Exception));
                    Assert.IsInstanceOfType(exception, typeof(CreateException));
                }
            }
            else
            {
                Assert.Fail($"PermissionService is null");
            }
        }
        catch (Exception ex)
        {
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task HandleEnabledPermissionModuleAsync()
    {
        try
        {
            var updated = await _permissionService.HandleEnabledPermissionModuleAsync(_permissionModule, false);
            Assert.IsTrue(updated);
            Assert.IsFalse(_permissionModule.Enabled);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task GetPermissionModuleByGuidAsync_Success()
    {
        try
        {

            if (_permissionService != null)
            {
                var permission = await _permissionService.GetPermissionModuleByGuidAsync(_permissionModule.Guid);
                Assert.IsNotNull(permission);
                Assert.IsInstanceOfType(permission, typeof(PermissionModule));
                Assert.AreEqual(_permissionModule.Name, permission.Name);
                Assert.AreEqual( _permissionModule.Enabled, permission.Enabled);
            }
            else
            {
                Assert.Fail($"PermissionService is null");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task GetPermissionModuleByNameAsync_Success()
    {
        try
        {

            if (_permissionService != null)
            {
                var permission = await _permissionService.GetPermissionModuleByNameAsync(_permissionModule.Name);
                Assert.IsNotNull(permission);
                Assert.IsInstanceOfType(permission, typeof(PermissionModule));
                Assert.AreEqual(_permissionModule.Guid, permission.Guid);
                Assert.AreEqual( _permissionModule.Enabled, permission.Enabled);
            }
            else
            {
                Assert.Fail($"PermissionService is null");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    #endregion


    #region "PermissionOperation"

    [TestMethod]
    public async Task GetPermissionOperationByGuidAsync_Null()
    {
        try
        {

            if (_permissionService != null)
            {
                var permission = await _permissionService.GetPermissionOperationByGuidAsync(Guid.NewGuid().ToString());
                Assert.IsNull(permission);
            }
            else
            {
                Assert.Fail($"PermissionService is null");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task GetPermissionOperationByNameAsync_Null()
    {
        try
        {

            if (_permissionService != null)
            {
                var permission = await _permissionService.GetPermissionOperationByNameAsync(Guid.NewGuid().ToString());
                Assert.IsNull(permission);
            }
            else
            {
                Assert.Fail($"PermissionService is null");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task CreatePermissionOperationAsync_Success()
    {
        try
        {
            var permission = await _permissionService.CreatePermissionOperationAsync(_permissionOperation.Name);
            Assert.IsInstanceOfType(permission, typeof(PermissionOperation));
            Assert.IsNotNull(permission);
            Assert.AreEqual(_permissionOperation.Name, permission.Name);
            _permissionOperation = permission;

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task CreatePermissionOperationAsync_Exception()
    {
        try
        {
            var exceptionPermissionService = TestUtils.CreatePermissionServiceException();

            if (exceptionPermissionService != null)
            {
                try
                {
                    var permission = await exceptionPermissionService.CreatePermissionOperationAsync(_permissionOperation.Name);
                    Assert.Fail($"Expected exception instead of response: {permission?.Guid}");
                }
                catch (Exception exception)
                {
                    Assert.IsInstanceOfType(exception, typeof(Exception));
                    Assert.IsInstanceOfType(exception, typeof(CreateException));
                }
            }
            else
            {
                Assert.Fail($"PermissionService is null");
            }
        }
        catch (Exception ex)
        {
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task GetPermissionOperationByGuidAsync_Success()
    {
        try
        {

            if (_permissionService != null)
            {
                var permission = await _permissionService.GetPermissionOperationByGuidAsync(_permissionOperation.Guid);
                Assert.IsNotNull(permission);
                Assert.IsInstanceOfType(permission, typeof(PermissionOperation));
                Assert.AreEqual(_permissionOperation.Name, permission.Name);
            }
            else
            {
                Assert.Fail($"PermissionService is null");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task GetPermissionOperationByNameAsync_Success()
    {
        try
        {

            if (_permissionService != null)
            {
                var permission = await _permissionService.GetPermissionOperationByNameAsync(_permissionOperation.Name);
                Assert.IsNotNull(permission);
                Assert.IsInstanceOfType(permission, typeof(PermissionOperation));
                Assert.AreEqual(_permissionOperation.Guid, permission.Guid);
            }
            else
            {
                Assert.Fail($"PermissionService is null");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    #endregion


    #region "PermissionSystemModule"

    [TestMethod]
    public async Task GetPermissionSystemModuleByGuidAsync_Null()
    {
        try
        {

            if (_permissionService != null)
            {
                var permission = await _permissionService.GetPermissionSystemModuleByGuidAsync(Guid.NewGuid().ToString());
                Assert.IsNull(permission);
            }
            else
            {
                Assert.Fail($"PermissionService is null");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task CreatePermissionSystemModuleAsync_Success()
    {
        try
        {
            var permission = await _permissionService.CreatePermissionSystemModuleAsync(_permissionSystem, _permissionModule, true);
            Assert.IsInstanceOfType(permission, typeof(PermissionSystemModule));
            Assert.IsNotNull(permission);
            Assert.IsTrue(permission.Enabled);
            _permissionSystemModule = permission;

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task CreatePermissionSystemModuleAsync_Exception()
    {
        try
        {
            var exceptionPermissionService = TestUtils.CreatePermissionServiceException();

            if (exceptionPermissionService != null)
            {
                try
                {
                    var permission = await exceptionPermissionService.CreatePermissionSystemModuleAsync(_permissionSystem, _permissionModule, true);
                    Assert.Fail($"Expected exception instead of response: {permission?.Guid}");
                }
                catch (Exception exception)
                {
                    Assert.IsInstanceOfType(exception, typeof(Exception));
                    Assert.IsInstanceOfType(exception, typeof(CreateException));
                }
            }
            else
            {
                Assert.Fail($"PermissionService is null");
            }
        }
        catch (Exception ex)
        {
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task HandleEnabledPermissionSystemModuleAsync()
    {
        try
        {
            var updated = await _permissionService.HandleEnabledPermissionSystemModuleAsync(_permissionSystemModule, false);
            Assert.IsTrue(updated);
            Assert.IsFalse(_permissionSystemModule.Enabled);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task GetPermissionSystemModuleByGuidAsync_Success()
    {
        try
        {

            if (_permissionService != null)
            {
                var permission = await _permissionService.GetPermissionSystemModuleByGuidAsync(_permissionSystemModule.Guid);
                Assert.IsNotNull(permission);
                Assert.IsInstanceOfType(permission, typeof(PermissionSystemModule));
                Assert.AreEqual( _permissionSystemModule.Enabled, permission.Enabled);
            }
            else
            {
                Assert.Fail($"PermissionService is null");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    #endregion


    #region "PermissionSystemModuleOperation"

    [TestMethod]
    public async Task GetPermissionSystemModuleOperationByGuidAsync_Null()
    {
        try
        {

            if (_permissionService != null)
            {
                var permission = await _permissionService.GetPermissionSystemModuleOperationByGuidAsync(Guid.NewGuid().ToString());
                Assert.IsNull(permission);
            }
            else
            {
                Assert.Fail($"PermissionService is null");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task CreatePermissionSystemModuleOperationAsync_Success()
    {
        try
        {
            var permission = await _permissionService.CreatePermissionSystemModuleOperationAsync(_permissionSystemModule, _permissionOperation, true);
            Assert.IsInstanceOfType(permission, typeof(PermissionSystemModuleOperation));
            Assert.IsNotNull(permission);
            Assert.IsTrue(permission.Enabled);
            _permissionSystemModuleOperation = permission;

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task CreatePermissionSystemModuleOperationAsync_Exception()
    {
        try
        {
            var exceptionPermissionService = TestUtils.CreatePermissionServiceException();

            if (exceptionPermissionService != null)
            {
                try
                {
                    var permission = await exceptionPermissionService.CreatePermissionSystemModuleOperationAsync(_permissionSystemModule, _permissionOperation, true);
                    Assert.Fail($"Expected exception instead of response: {permission?.Guid}");
                }
                catch (Exception exception)
                {
                    Assert.IsInstanceOfType(exception, typeof(Exception));
                    Assert.IsInstanceOfType(exception, typeof(CreateException));
                }
            }
            else
            {
                Assert.Fail($"PermissionService is null");
            }
        }
        catch (Exception ex)
        {
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task HandleEnabledPermissionSystemModuleOperationAsync()
    {
        try
        {
            var updated = await _permissionService.HandleEnabledPermissionSystemModuleOperationAsync(_permissionSystemModuleOperation, false);
            Assert.IsTrue(updated);
            Assert.IsFalse(_permissionSystemModuleOperation.Enabled);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task GetPermissionSystemModuleOperationByGuidAsync_Success()
    {
        try
        {

            if (_permissionService != null)
            {
                var permission = await _permissionService.GetPermissionSystemModuleOperationByGuidAsync(_permissionSystemModuleOperation.Guid);
                Assert.IsNotNull(permission);
                Assert.IsInstanceOfType(permission, typeof(PermissionSystemModuleOperation));
                Assert.AreEqual(_permissionSystemModuleOperation.Enabled, permission.Enabled);
            }
            else
            {
                Assert.Fail($"PermissionService is null");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    #endregion


    #region "RolePermissionSystemModuleOperation"

    [TestMethod]
    public async Task GetRolePermissionSystemModuleOperationByGuidAsync_Null()
    {
        try
        {

            if (_permissionService != null)
            {
                var permission = await _permissionService.GetRolePermissionSystemModuleOperationByGuidAsync(Guid.NewGuid().ToString());
                Assert.IsNull(permission);
            }
            else
            {
                Assert.Fail($"PermissionService is null");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task CreateRolePermissionSystemModuleOperationAsync_Success()
    {
        try
        {
            var expectedUser = ModelsInit.CreateUser();
            Role role = new()
            {
                Name = expectedUser.Role?.Name ?? String.Empty,
                IsNotEditable = expectedUser.Role?.IsNotEditable ?? false,
                Guid = expectedUser.Role?.Guid ?? String.Empty
            };

            var permission = await _permissionService.CreateRolePermissionSystemModuleOperationAsync(role, _permissionSystemModuleOperation, true);
            Assert.IsInstanceOfType(permission, typeof(RolePermissionSystemModuleOperation));
            Assert.IsNotNull(permission);
            Assert.IsTrue(permission.Active);
            _rolePermissionSystemModuleOperation = permission;

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task CreateRolePermissionSystemModuleOperationAsync_Exception()
    {
        try
        {
            var exceptionPermissionService = TestUtils.CreatePermissionServiceException();

            if (exceptionPermissionService != null)
            {
                try
                {
                    var expectedUser = ModelsInit.CreateUser();
                    Role role = new()
                    {
                        Name = expectedUser.Role?.Name ?? String.Empty,
                        IsNotEditable = expectedUser.Role?.IsNotEditable ?? false,
                        Guid = expectedUser.Role?.Guid ?? String.Empty
                    };

                    var permission = await exceptionPermissionService.CreateRolePermissionSystemModuleOperationAsync(role, _permissionSystemModuleOperation, true);
                    Assert.Fail($"Expected exception instead of response: {permission?.Guid}");
                }
                catch (Exception exception)
                {
                    Assert.IsInstanceOfType(exception, typeof(Exception));
                    Assert.IsInstanceOfType(exception, typeof(CreateException));
                }
            }
            else
            {
                Assert.Fail($"PermissionService is null");
            }
        }
        catch (Exception ex)
        {
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task HandleEnabledRolePermissionSystemModuleOperationAsync()
    {
        try
        {
            var updated = await _permissionService.HandleEnabledRolePermissionSystemModuleOperationAsync(_rolePermissionSystemModuleOperation, false);
            Assert.IsTrue(updated);
            Assert.IsFalse(_rolePermissionSystemModuleOperation.Active);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task GetRolePermissionSystemModuleOperationByGuidAsync_Success()
    {
        try
        {

            if (_permissionService != null)
            {
                var permission = await _permissionService.GetRolePermissionSystemModuleOperationByGuidAsync(_rolePermissionSystemModuleOperation.Guid);
                Assert.IsNotNull(permission);
                Assert.IsInstanceOfType(permission, typeof(RolePermissionSystemModuleOperation));
                Assert.AreEqual( _rolePermissionSystemModuleOperation.Active, permission.Active);
            }
            else
            {
                Assert.Fail($"PermissionService is null");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    #endregion



    #region "DeletePermissions"

    [TestMethod]
    public async Task DeleteRolePermissionSystemModuleOperationAsync()
    {
        try
        {
            var deleted = await _permissionService.DeleteRolePermissionSystemModuleOperationAsync(_rolePermissionSystemModuleOperation);
            Assert.IsTrue(deleted);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task DeletePermissionSystemModuleOperationAsync()
    {
        try
        {
            var deleted = await _permissionService.DeletePermissionSystemModuleOperationAsync(_permissionSystemModuleOperation);
            Assert.IsTrue(deleted);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task DeletePermissionSystemModuleAsync()
    {
        try
        {
            var deleted = await _permissionService.DeletePermissionSystemModuleAsync(_permissionSystemModule);
            Assert.IsTrue(deleted);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task DeletePermissionSystemAsync()
    {
        try
        {
            var deleted = await _permissionService.DeletePermissionSystemAsync(_permissionSystem);
            Assert.IsTrue(deleted);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task DeletePermissionModuleAsync()
    {
        try
        {
            var deleted = await _permissionService.DeletePermissionModuleAsync(_permissionModule);
            Assert.IsTrue(deleted);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task DeletePermissionOperationAsync()
    {
        try
        {
            var deleted = await _permissionService.DeletePermissionOperationAsync(_permissionOperation);
            Assert.IsTrue(deleted);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    #endregion


    #region "CreatePermissions"

    [TestMethod]
    public void CreatePermissionsOnStartupAsync_Success()
    {
        try
        {

            if (_permissionService != null)
            {
                List<string> permissions = _permissionService.CreatePermissionsOnStartupAsync();
                Assert.IsNotNull(permissions);
                List<string> cleanedPermissions = new();

                foreach (var permission in permissions)
                {
                    cleanedPermissions.Add(permission
                        .Replace("Added new PermissionSystem => ", "")
                        .Replace("Added new PermissionModule => ", "")
                        .Replace("Added new PermissionOperation => ", "")
                        .Replace("Added new PermissionSystemModule => ", "")
                        .Replace("Added new PermissionSystemModuleOperation => ", "")
                        .Replace("Added new RolePermissionSystemModuleOperation => ", "")
                    );
                }
                Assert.IsTrue(cleanedPermissions.Contains("base"));

                Assert.IsTrue(cleanedPermissions.Contains("roles"));
                Assert.IsTrue(cleanedPermissions.Contains("users"));

                Assert.IsTrue(cleanedPermissions.Contains("create"));
                Assert.IsTrue(cleanedPermissions.Contains("read"));
                Assert.IsTrue(cleanedPermissions.Contains("update"));
                Assert.IsTrue(cleanedPermissions.Contains("delete"));
                Assert.IsTrue(cleanedPermissions.Contains("list"));
                Assert.IsTrue(cleanedPermissions.Contains("use"));

                Assert.IsTrue(cleanedPermissions.Contains("base.roles"));
                Assert.IsTrue(cleanedPermissions.Contains("base.users"));

                Assert.IsTrue(cleanedPermissions.Contains("base.roles.create"));
                Assert.IsTrue(cleanedPermissions.Contains("base.roles.read"));
                Assert.IsTrue(cleanedPermissions.Contains("base.roles.update"));
                Assert.IsTrue(cleanedPermissions.Contains("base.roles.delete"));
                Assert.IsTrue(cleanedPermissions.Contains("base.roles.list"));
                Assert.IsTrue(cleanedPermissions.Contains("base.roles.use"));

                Assert.IsTrue(cleanedPermissions.Contains("base.roles.create for role Admin"));
                Assert.IsTrue(cleanedPermissions.Contains("base.roles.update for role Admin"));
                Assert.IsTrue(cleanedPermissions.Contains("base.roles.delete for role Admin"));

                Assert.IsTrue(cleanedPermissions.Contains("base.users.create"));
                Assert.IsTrue(cleanedPermissions.Contains("base.users.read"));
                Assert.IsTrue(cleanedPermissions.Contains("base.users.update"));
                Assert.IsTrue(cleanedPermissions.Contains("base.users.delete"));
                Assert.IsTrue(cleanedPermissions.Contains("base.users.list"));

                Assert.IsFalse(cleanedPermissions.Contains("base.users.use"));

            }
            else
            {
                Assert.Fail($"PermissionService is null");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    #endregion
}



