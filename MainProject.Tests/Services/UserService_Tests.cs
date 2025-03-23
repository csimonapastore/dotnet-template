using BasicDotnetTemplate.MainProject.Services;
using BasicDotnetTemplate.MainProject.Models.Api.Data.User;
using BasicDotnetTemplate.MainProject.Models.Database.SqlServer;



namespace BasicDotnetTemplate.MainProject.Tests;

[TestClass]
public class UserService_Tests
{
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

    [TestMethod]
    public async Task CheckIfEmailIsValid_EmailNotExists()
    {
        try
        {
            var userService = TestUtils.CreateUserService();
            var user = ModelsInit.CreateUser();
            if (userService != null)
            {
                var valid = await userService.CheckIfEmailIsValid(user.Email);
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
            var roleService = TestUtils.CreateRoleService();
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
            Assert.IsTrue(expectedUser.FirstName == user.FirstName);
            Assert.IsTrue(expectedUser.LastName == user.LastName);
            Assert.IsTrue(expectedUser.Email == user.Email);
            Assert.IsTrue(expectedUser.Role?.Name == user.Role?.Name);

            // CheckIfEmailIsValid_CurrentUser
            var valid = await userService.CheckIfEmailIsValid(user.Email, user.Guid);
            Assert.IsTrue(valid);

            // CheckIfEmailIsValid_EmailAlreadyExists
            valid = await userService.CheckIfEmailIsValid(user.Email);
            Assert.IsFalse(valid);

            //GetUserByIdAsync
            var getUserById = await userService.GetUserByIdAsync(user.Id);
            Assert.IsNotNull(getUserById);
            Assert.IsTrue(getUserById.Id == user?.Id);

            //GetUserByGuidAsync
            var getUserByGuid = await userService.GetUserByGuidAsync(user.Guid);
            Assert.IsNotNull(getUserByGuid);
            Assert.IsTrue(getUserByGuid.Guid == user?.Guid);

            //DeleteUserAsync
            var deleted = await userService.DeleteUserAsync(user);
            Assert.IsTrue(deleted);

            //DeleteRoleAsync
            deleted = await roleService.DeleteRoleAsync(user.Role!);
            Assert.IsTrue(deleted);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

}




