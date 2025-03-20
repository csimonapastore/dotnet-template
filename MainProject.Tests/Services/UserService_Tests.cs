using BasicDotnetTemplate.MainProject.Services;
using BasicDotnetTemplate.MainProject.Models.Api.Data.User;
using BasicDotnetTemplate.MainProject.Models.Database.SqlServer;



namespace BasicDotnetTemplate.MainProject.Tests;

[TestClass]
public class UserService_Tests
{
    private static User _user = ModelsInit.CreateUser();
    private static Role _role = ModelsInit.CreateRole();

    [TestMethod]
    public void Inizialize()
    {
        try
        {
            var userService = TestUtils.CreateUserService();
            if (userService != null)
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

    // [TestInitialize]
    // public void Setup()
    // {
    //     _expectedUser = ModelsInit.CreateUser();
    //     _expectedRole = ModelsInit.CreateRole();
    //     _roleService = TestUtils.CreateRoleService();
    //     _userService = TestUtils.CreateUserService();
    // }

    // [TestMethod]
    // public void Inizialize()
    // {
    //     try
    //     {
    //         if (_userService != null)
    //         {
    //             Assert.IsInstanceOfType(_userService, typeof(UserService));
    //         }
    //         else
    //         {
    //             Assert.Fail($"UserService is null");
    //         }
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine(ex.InnerException);
    //         Assert.Fail($"An exception was thrown: {ex}");
    //     }
    // }

    [TestMethod]
    public async Task GetUserByUsernameAndPassword_Null()
    {
        try
        {
            var userService = TestUtils.CreateUserService();
            var testString = "test";
            if (userService != null)
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
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    // // TODO
    // // [TestMethod]
    // public async Task GetUserByUsernameAndPassword_Success()
    // {
    //     try
    //     {
    //         var testEmail = "test@email.it";
    //         var testPassword = "password";
    //         if (_userService != null)
    //         {
    //             var user = await _userService.GetUserByUsernameAndPassword(testEmail, testPassword);
    //             Assert.IsTrue(user != null);
    //             Assert.IsTrue(user.Email == testEmail);
    //         }
    //         else
    //         {
    //             Assert.Fail($"UserService is null");
    //         }
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine(ex.InnerException);
    //         Assert.Fail($"An exception was thrown: {ex}");
    //     }
    // }

    [TestMethod]
    public async Task CheckIfEmailIsValid_EmailNotExists()
    {
        try
        {
            var userService = TestUtils.CreateUserService();
            if (userService != null)
            {
                var valid = await userService.CheckIfEmailIsValid(_user.Email ?? String.Empty);
                Assert.IsTrue(valid);
            }
            else
            {
                Assert.Fail($"UserService is null");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task CreateUserAsync_Success()
    {
        try
        {
            var userService = TestUtils.CreateUserService();
            var expectedUser = ModelsInit.CreateUser();

            CreateUserRequestData data = new CreateUserRequestData()
            {
                FirstName = expectedUser.FirstName ?? String.Empty,
                LastName = expectedUser.LastName ?? String.Empty,
                Email = expectedUser.Email ?? String.Empty
            };

            Role role = new()
            {
                Name = expectedUser.Role?.Name ?? String.Empty,
                IsNotEditable = expectedUser.Role?.IsNotEditable ?? false,
                Guid = expectedUser.Role?.Guid ?? String.Empty
            };

            var user = await userService.CreateUserAsync(data, role);
            Assert.IsInstanceOfType(user, typeof(User));
            Assert.IsNotNull(user);
            Assert.IsTrue(expectedUser.FirstName == user.FirstName);
            Assert.IsTrue(expectedUser.LastName == user.LastName);
            Assert.IsTrue(expectedUser.Email == user.Email);
            Assert.IsTrue(expectedUser.Role?.Name == user.Role?.Name);
            _user = user;
            _role = user.Role;

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task CheckIfEmailIsValid_EmailCurrentUser()
    {
        try
        {
            var userService = TestUtils.CreateUserService();
            if (userService != null)
            {
                var valid = await userService.CheckIfEmailIsValid(_user.Email ?? String.Empty, _user.Guid ?? String.Empty);
                Assert.IsTrue(valid);
            }
            else
            {
                Assert.Fail($"UserService is null");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task CheckIfEmailIsValid_EmailAlreadyExists()
    {
        try
        {
            var userService = TestUtils.CreateUserService();
            if (userService != null)
            {
                var valid = await userService.CheckIfEmailIsValid(_user.Email ?? String.Empty);
                Assert.IsFalse(valid);
            }
            else
            {
                Assert.Fail($"UserService is null");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task GetUserByIdAsync()
    {
        try
        {
            var userService = TestUtils.CreateUserService();
            if (userService != null)
            {
                var user = await userService.GetUserByIdAsync(_user.Id);
                Assert.IsNotNull(user);
                Assert.IsTrue(user.Id == _user?.Id);
            }
            else
            {
                Assert.Fail($"UserService is null");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task GetUserByGuidAsync()
    {
        try
        {
            var userService = TestUtils.CreateUserService();
            if (userService != null)
            {
                var user = await userService.GetUserByGuidAsync(_user.Guid ?? String.Empty);
                Assert.IsNotNull(user);
                Assert.IsTrue(user.Guid == _user?.Guid);
            }
            else
            {
                Assert.Fail($"UserService is null");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task DeleteUser()
    {
        try
        {
            var userService = TestUtils.CreateUserService();
            if (userService != null)
            {
                var user = await userService.GetUserByGuidAsync(_user.Guid ?? String.Empty);
                Assert.IsNotNull(user);
                var deleted = await userService.DeleteUserAsync(user);
                Assert.IsTrue(deleted);
            }
            else
            {
                Assert.Fail($"UserService is null");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }



    [TestMethod]
    public static async Task CleanupAsync()
    {
        var roleService = TestUtils.CreateRoleService();
        var role = await roleService.GetRoleByGuidAsync(_role.Guid ?? String.Empty);
        Assert.IsNotNull(role);
        var deleted = await roleService.DeleteRoleAsync(role);
        Assert.IsTrue(deleted);
    }

}




