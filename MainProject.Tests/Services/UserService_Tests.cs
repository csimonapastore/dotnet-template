using BasicDotnetTemplate.MainProject.Services;
using BasicDotnetTemplate.MainProject.Models.Api.Data.User;
using BasicDotnetTemplate.MainProject.Models.Database.SqlServer;
using Newtonsoft.Json;
using BasicDotnetTemplate.MainProject.Models.Api.Common.Exceptions;



namespace BasicDotnetTemplate.MainProject.Tests;

[TestClass]
public class UserService_Tests
{
    private static User _user = ModelsInit.CreateUser();
    private static UserService _userService = TestUtils.CreateUserService();

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

    [TestMethod]
    public async Task CheckIfEmailIsValid_EmailNotExists()
    {
        try
        {
            if (_userService != null)
            {
                var valid = await _userService.CheckIfEmailIsValid(_user.Email ?? String.Empty);
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

            var expectedUser = ModelsInit.CreateUser();

            CreateUserRequestData data = new CreateUserRequestData()
            {
                FirstName = expectedUser.FirstName ?? String.Empty,
                LastName = expectedUser.LastName ?? String.Empty,
                Email = expectedUser.Email ?? String.Empty,
                Password = "Password"
            };

            Role role = new()
            {
                Name = expectedUser.Role?.Name ?? String.Empty,
                IsNotEditable = expectedUser.Role?.IsNotEditable ?? false,
                Guid = expectedUser.Role?.Guid ?? String.Empty
            };

            var user = await _userService.CreateUserAsync(data, role);
            Assert.IsInstanceOfType(user, typeof(User));
            Assert.IsNotNull(user);
            Assert.AreEqual(expectedUser.FirstName, user.FirstName);
            Assert.AreEqual(expectedUser.LastName, user.LastName);
            Assert.AreEqual(expectedUser.Email, user.Email);
            Assert.AreEqual(expectedUser.Role?.Name, user.Role?.Name);
            Assert.AreEqual(10, user.PasswordIterations);
            Assert.IsNotNull(expectedUser.PasswordSalt);
            Assert.IsNotNull(expectedUser.PasswordPepper);
            Assert.IsNotNull(expectedUser.Password);
            _user = user;

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task GetUserByUsernameAndPassword_Null()
    {
        try
        {
            if (_userService != null)
            {
                var user = await _userService.GetUserByUsernameAndPassword(_user.Email, "WrongPassword");
                Assert.IsNull(user);
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
    public async Task GetUserByUsernameAndPassword_Success()
    {
        try
        {
            var password = "Password";
            if (_userService != null)
            {
                var user = await _userService.GetUserByUsernameAndPassword(_user.Email, password);
                Assert.IsNotNull(user);
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
    public async Task CreateUserAsync_Exception()
    {
        try
        {
            var expectedUser = ModelsInit.CreateUser();

            CreateUserRequestData data = new CreateUserRequestData()
            {
                FirstName = expectedUser.FirstName ?? String.Empty,
                LastName = expectedUser.LastName ?? String.Empty,
                Email = expectedUser.Email ?? String.Empty,
                Password = expectedUser.Password ?? String.Empty
            };

            Role role = new()
            {
                Name = expectedUser.Role?.Name ?? String.Empty,
                IsNotEditable = expectedUser.Role?.IsNotEditable ?? false,
                Guid = expectedUser.Role?.Guid ?? String.Empty
            };

            var exceptionUserService = TestUtils.CreateUserServiceException();

            if (exceptionUserService != null)
            {
                try
                {
                    var user = await exceptionUserService.CreateUserAsync(data, role);
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
                Assert.Fail($"UserService is null");
            }
        }
        catch (Exception ex)
        {
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task CheckIfEmailIsValid_EmailCurrentUser()
    {
        try
        {

            if (_userService != null)
            {
                var valid = await _userService.CheckIfEmailIsValid(_user.Email ?? String.Empty, _user.Guid ?? String.Empty);
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

            if (_userService != null)
            {
                var valid = await _userService.CheckIfEmailIsValid(_user.Email ?? String.Empty);
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

            if (_userService != null)
            {
                var user = await _userService.GetUserByIdAsync(_user.Id);
                Assert.IsNotNull(user);
                Assert.AreEqual(user.Id, _user?.Id);
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

            if (_userService != null)
            {
                var user = await _userService.GetUserByGuidAsync(_user.Guid ?? String.Empty);
                Assert.IsNotNull(user);
                Assert.AreEqual(user.Guid, _user?.Guid);
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
    public async Task UpdateUserAsync_Success()
    {
        try
        {
            UpdateUserRequestData data = new UpdateUserRequestData()
            {
                FirstName = "ChangedUserFirstName",
                LastName = "ChangedUserLastName"
            };

            if (_userService != null)
            {
                Assert.IsNotNull(_user);
                var user = await _userService.UpdateUserAsync(data, _user!);
                Assert.IsInstanceOfType(user, typeof(User));
                Assert.IsNotNull(user);
                Assert.AreEqual(data.FirstName, user.FirstName);
                Assert.AreEqual(data.LastName, user.LastName);
                _user = user;
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
    public async Task UpdateUserAsync_Exception()
    {
        try
        {
            UpdateUserRequestData data = new UpdateUserRequestData()
            {
                FirstName = "ChangedUserFirstName",
                LastName = "ChangedUserLastName"
            };

            var exceptionUserService = TestUtils.CreateUserServiceException();

            if (exceptionUserService != null)
            {
                Assert.IsNotNull(_user);
                var user = await exceptionUserService.UpdateUserAsync(data, _user!);
                Assert.Fail($"Expected exception instead of response: {user?.Guid}");

            }
            else
            {
                Assert.Fail($"UserService is null");
            }
        }
        catch (Exception ex)
        {
            Assert.IsInstanceOfType(ex, typeof(Exception));
        }
    }


    [TestMethod]
    public async Task UpdateUserPasswordAsync_Success()
    {
        try
        {
            if (_userService != null)
            {
                Assert.IsNotNull(_user);
                var oldPassword = _user.Password;
                var user = await _userService.UpdateUserPasswordAsync(_user!, "this-is-a-new-password");
                Assert.IsInstanceOfType(user, typeof(User));
                Assert.IsNotNull(user);
                Assert.AreNotEqual(user.Password, oldPassword);
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
    public async Task UpdateUserPasswordAsync_Exception()
    {
        try
        {
            var exceptionUserService = TestUtils.CreateUserServiceException();

            if (exceptionUserService != null)
            {
                Assert.IsNotNull(_user);
                var user = await exceptionUserService.UpdateUserPasswordAsync(_user!, "this-is-a-new-password");
                Assert.Fail($"Expected exception instead of response: {user?.Guid}");

            }
            else
            {
                Assert.Fail($"UserService is null");
            }
        }
        catch (Exception ex)
        {
            Assert.IsInstanceOfType(ex, typeof(Exception));
        }
    }

    [TestMethod]
    public async Task UpdateUserRoleAsync_Success()
    {
        try
        {
            if (_userService != null)
            {
                Assert.IsNotNull(_user);
                Role role = new()
                {
                    Name = "NewRole",
                    IsNotEditable = false,
                    Guid = Guid.NewGuid().ToString()
                };

                var oldRole = _user.Role;
                var user = await _userService.UpdateUserRoleAsync(_user!, role);
                Assert.IsInstanceOfType(user, typeof(User));
                Assert.IsNotNull(user);
                Assert.AreNotEqual(user.Role?.Id, oldRole?.Id);
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
    public async Task UpdateUserRoleAsync_Exception()
    {
        try
        {
            var exceptionUserService = TestUtils.CreateUserServiceException();

            if (exceptionUserService != null)
            {
                Assert.IsNotNull(_user);
                Role role = new()
                {
                    Name = "NewRole",
                    IsNotEditable = false,
                    Guid = Guid.NewGuid().ToString()
                };
                var user = await exceptionUserService.UpdateUserRoleAsync(_user!, role);
                Assert.Fail($"Expected exception instead of response: {user?.Guid}");

            }
            else
            {
                Assert.Fail($"UserService is null");
            }
        }
        catch (Exception ex)
        {
            Assert.IsInstanceOfType(ex, typeof(Exception));
        }
    }

    [TestMethod]
    public async Task DeleteUser()
    {
        try
        {

            if (_userService != null)
            {
                var user = await _userService.GetUserByGuidAsync(_user.Guid ?? String.Empty);
                Assert.IsNotNull(user);
                var deleted = await _userService.DeleteUserAsync(user);
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


}



