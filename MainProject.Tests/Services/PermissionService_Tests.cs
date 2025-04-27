using BasicDotnetTemplate.MainProject.Services;
using BasicDotnetTemplate.MainProject.Models.Api.Data.User;
using BasicDotnetTemplate.MainProject.Models.Database.SqlServer;
using Newtonsoft.Json;
using BasicDotnetTemplate.MainProject.Models.Api.Common.Exceptions;


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
                Assert.IsTrue(permission == null);
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
                Assert.IsTrue(permission == null);
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
            Assert.IsTrue(permission.Name == _permissionSystem.Name);
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
            Assert.IsTrue(!_permissionSystem.Enabled);

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
                Assert.IsTrue(permission.Name == _permissionSystem.Name);
                Assert.IsTrue(permission.Enabled == _permissionSystem.Enabled);
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
                Assert.IsTrue(permission.Guid == _permissionSystem.Guid);
                Assert.IsTrue(permission.Enabled == _permissionSystem.Enabled);
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
                Assert.IsTrue(permission == null);
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
                Assert.IsTrue(permission == null);
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
            Assert.IsTrue(permission.Name == _permissionModule.Name);
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
            Assert.IsTrue(!_permissionModule.Enabled);

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
                Assert.IsTrue(permission.Name == _permissionModule.Name);
                Assert.IsTrue(permission.Enabled == _permissionModule.Enabled);
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
                Assert.IsTrue(permission.Guid == _permissionModule.Guid);
                Assert.IsTrue(permission.Enabled == _permissionModule.Enabled);
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
                Assert.IsTrue(permission == null);
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
                Assert.IsTrue(permission == null);
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
            Assert.IsTrue(permission.Name == _permissionOperation.Name);
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
                Assert.IsTrue(permission.Name == _permissionOperation.Name);
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
                Assert.IsTrue(permission.Guid == _permissionOperation.Guid);
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
                Assert.IsTrue(permission == null);
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
                    var user = await exceptionPermissionService.CreatePermissionSystemModuleAsync(_permissionSystem, _permissionModule, true);
                    Assert.Fail($"Expected exception instead of response: {user?.Guid}");
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
            Assert.IsTrue(!_permissionSystemModule.Enabled);

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
                Assert.IsTrue(permission.Enabled == _permissionSystemModule.Enabled);
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

}



