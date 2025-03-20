using BasicDotnetTemplate.MainProject.Services;
using BasicDotnetTemplate.MainProject.Models.Api.Data.User;
using BasicDotnetTemplate.MainProject.Models.Database.SqlServer;



namespace BasicDotnetTemplate.MainProject.Tests;

[TestClass]
public class UserService_Tests
{
    private static Role? _expectedRole;
    private static User? _user;
    private static Role? _role;
    private static User? _expectedUser;

    private static RoleService? _roleService;
    private static UserService? _userService;

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
            _expectedUser = ModelsInit.CreateUser();
            _userService = TestUtils.CreateUserService();
            _roleService = TestUtils.CreateRoleService();

            var testString = "test";
            if (_userService != null)
            {
                var user = await _userService.GetUserByUsernameAndPassword(testString, testString);
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

    // [TestMethod]
    // public async Task CheckIfEmailIsValid_EmailNotExists()
    // {
    //     try
    //     {
    //         if (_userService != null)
    //         {
    //             var valid = await _userService.CheckIfEmailIsValid(_expectedUser?.Email ?? String.Empty);
    //             Assert.IsTrue(valid);
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

    // [TestMethod]
    // public async Task CreateUserData()
    // {
    //     try
    //     {
    //         CreateUserRequestData data = new CreateUserRequestData()
    //         {
    //             FirstName = _expectedUser?.FirstName ?? String.Empty,
    //             LastName = _expectedUser?.LastName ?? String.Empty,
    //             Email = _expectedUser?.Email ?? String.Empty
    //         };

    //         Role role = new Role()
    //         {
    //             Name = _expectedUser?.Role?.Name ?? String.Empty,
    //             IsNotEditable = _expectedUser?.Role?.IsNotEditable ?? false,
    //             Guid = _expectedUser?.Role?.Guid ?? String.Empty
    //         };

    //         if (_userService != null)
    //         {
    //             var user = await _userService.CreateUserAsync(data, role);
    //             Assert.IsInstanceOfType(user, typeof(User));
    //             Assert.IsNotNull(user);
    //             Assert.IsTrue(_expectedUser?.FirstName == user.FirstName);
    //             Assert.IsTrue(_expectedUser?.LastName == user.LastName);
    //             Assert.IsTrue(_expectedUser?.Email == user.Email);
    //             Assert.IsTrue(_expectedUser?.Role?.Name == user.Role?.Name);
    //             _user = user;
    //             _role = user.Role;
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

    // [TestMethod]
    // public async Task CheckIfEmailIsValid_EmailCurrentUser()
    // {
    //     try
    //     {
    //         if (_userService != null)
    //         {
    //             var valid = await _userService.CheckIfEmailIsValid(_expectedUser?.Email ?? String.Empty, _user?.Guid ?? String.Empty);
    //             Assert.IsTrue(valid);
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

    // [TestMethod]
    // public async Task CheckIfEmailIsValid_EmailAlreadyExists()
    // {
    //     try
    //     {
    //         if (_userService != null)
    //         {
    //             var valid = await _userService.CheckIfEmailIsValid(_expectedUser?.Email ?? String.Empty);
    //             Assert.IsFalse(valid);
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

    // [TestMethod]
    // public async Task GetUserByIdAsync()
    // {
    //     try
    //     {
    //         if (_userService != null)
    //         {
    //             var user = await _userService.GetUserByIdAsync(_user?.Id ?? 0);
    //             Assert.IsNotNull(user);
    //             Assert.IsTrue(user.Id == _user?.Id);
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

    // [TestMethod]
    // public async Task GetUserByGuidAsync()
    // {
    //     try
    //     {
    //         if (_userService != null)
    //         {
    //             var user = await _userService.GetUserByGuidAsync(_user?.Guid ?? String.Empty);
    //             Assert.IsNotNull(user);
    //             Assert.IsTrue(user.Guid == _user?.Guid);
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

    // [TestMethod]
    // public async Task DeleteUser()
    // {
    //     try
    //     {
    //         if (_userService != null)
    //         {
    //             var user = await _userService.GetUserByGuidAsync(_user?.Guid ?? String.Empty);
    //             Assert.IsNotNull(user);
    //             var deleted = await _userService.DeleteUserAsync(user);
    //             Assert.IsTrue(deleted);
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



    // [TestCleanup]
    // public static async Task CleanupAsync()
    // {
    //     var role = await _roleService?.GetRoleByGuidAsync(_role?.Guid ?? String.Empty);
    //     Assert.IsNotNull(role);
    //     var deleted = await _roleService?.DeleteRoleAsync(role);
    //     Assert.IsTrue(deleted);
    // }

}




