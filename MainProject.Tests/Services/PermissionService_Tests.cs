using BasicDotnetTemplate.MainProject.Services;
using BasicDotnetTemplate.MainProject.Models.Api.Data.User;
using BasicDotnetTemplate.MainProject.Models.Database.SqlServer;
using Newtonsoft.Json;



namespace BasicDotnetTemplate.MainProject.Tests;

[TestClass]
public class PermissionService_Tests
{
    private static PermissionService _permissionService = TestUtils.CreatePermissionService();
    private static string _name = "TEST";
    private static PermissionSystem _permissionSystem = new PermissionSystem()
    {
        Name = _name,
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
            var permission = await _permissionService.CreatePermissionSystemAsync(_name, true);
            Assert.IsInstanceOfType(permission, typeof(PermissionSystem));
            Assert.IsNotNull(permission);
            Assert.IsTrue(permission.Name == _name);
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
                    var user = await exceptionPermissionService.CreatePermissionSystemAsync(_name, true);
                    Assert.Fail($"Expected exception instead of response: {user?.Guid}");
                }
                catch (Exception exception)
                {
                    Assert.IsInstanceOfType(exception, typeof(Exception));
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
    public async Task HandleEnabledPermissionSystem()
    {
        try
        {
            var updated = await _permissionService.HandleEnabledPermissionSystem(_permissionSystem, false);
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
    // [TestMethod]
    // public async Task GetUserByUsernameAndPassword_Null()
    // {
    //     try
    //     {
    //         var testString = "test";
    //         if (_userService != null)
    //         {
    //             var user = await _userService.GetUserByUsernameAndPassword(testString, testString);
    //             Assert.IsTrue(user == null);
    //         }
    //         else
    //         {
    //             Assert.Fail($"PermissionService is null");
    //         }
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine(ex.InnerException);
    //         Assert.Fail($"An exception was thrown: {ex}");
    //     }
    // }

    // [TestMethod]
    // public async Task CheckIfEmailIsValid_EmailNotExists()
    // {
    //     try
    //     {
    //         if (_userService != null)
    //         {
    //             var valid = await _userService.CheckIfEmailIsValid(_user.Email ?? String.Empty);
    //             Assert.IsTrue(valid);
    //         }
    //         else
    //         {
    //             Assert.Fail($"PermissionService is null");
    //         }
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine(ex.InnerException);
    //         Assert.Fail($"An exception was thrown: {ex}");
    //     }
    // }



    // [TestMethod]
    // public async Task CheckIfEmailIsValid_EmailCurrentUser()
    // {
    //     try
    //     {

    //         if (_userService != null)
    //         {
    //             var valid = await _userService.CheckIfEmailIsValid(_user.Email ?? String.Empty, _user.Guid ?? String.Empty);
    //             Assert.IsTrue(valid);
    //         }
    //         else
    //         {
    //             Assert.Fail($"PermissionService is null");
    //         }
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine(ex.InnerException);
    //         Assert.Fail($"An exception was thrown: {ex}");
    //     }
    // }

    // [TestMethod]
    // public async Task CheckIfEmailIsValid_EmailAlreadyExists()
    // {
    //     try
    //     {

    //         if (_userService != null)
    //         {
    //             var valid = await _userService.CheckIfEmailIsValid(_user.Email ?? String.Empty);
    //             Assert.IsFalse(valid);
    //         }
    //         else
    //         {
    //             Assert.Fail($"PermissionService is null");
    //         }
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine(ex.InnerException);
    //         Assert.Fail($"An exception was thrown: {ex}");
    //     }
    // }



    // [TestMethod]
    // public async Task GetUserByGuidAsync()
    // {
    //     try
    //     {

    //         if (_userService != null)
    //         {
    //             var user = await _userService.GetUserByGuidAsync(_user.Guid ?? String.Empty);
    //             Assert.IsNotNull(user);
    //             Assert.IsTrue(user.Guid == _user?.Guid);
    //         }
    //         else
    //         {
    //             Assert.Fail($"PermissionService is null");
    //         }
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine(ex.InnerException);
    //         Assert.Fail($"An exception was thrown: {ex}");
    //     }
    // }

    // [TestMethod]
    // public async Task DeleteUser()
    // {
    //     try
    //     {

    //         if (_userService != null)
    //         {
    //             var user = await _userService.GetUserByGuidAsync(_user.Guid ?? String.Empty);
    //             Assert.IsNotNull(user);
    //             var deleted = await _userService.DeleteUserAsync(user);
    //             Assert.IsTrue(deleted);
    //         }
    //         else
    //         {
    //             Assert.Fail($"PermissionService is null");
    //         }
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine(ex.InnerException);
    //         Assert.Fail($"An exception was thrown: {ex}");
    //     }
    // }


#region "DeletePermissions"
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
#endregion

}



