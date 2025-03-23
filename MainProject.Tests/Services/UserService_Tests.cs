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
    public async Task CreateUserAsync_Success()
    {
        try
        {
            var userService = TestUtils.CreateUserService();
            var roleService = TestUtils.CreateRoleService();
            var expectedUser = ModelsInit.CreateUser();

            var valid = await userService.CheckIfEmailIsValid(expectedUser.Email ?? String.Empty);
            Assert.IsTrue(valid);

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

            var user_role = await roleService.GetRoleByGuidAsync(user.Role?.Guid ?? String.Empty);


            valid = await userService.CheckIfEmailIsValid(user.Email, user.Guid);
            Assert.IsTrue(valid);

            valid = await userService.CheckIfEmailIsValid(user.Email, Guid.NewGuid().ToString());
            Assert.IsFalse(valid);


            var user_by_id = await userService.GetUserByIdAsync(user.Id);
            Assert.IsNotNull(user_by_id);
            Assert.IsTrue(user.Id == user_by_id.Id);


            var user_by_guid = await userService.GetUserByGuidAsync(user.Guid);
            Assert.IsNotNull(user_by_guid);
            Assert.IsTrue(user.Guid == user_by_guid.Guid);


            var deleted = await userService.DeleteUserAsync(user);
            Assert.IsTrue(deleted);


            Assert.IsNotNull(user_role);
            deleted = await roleService.DeleteRoleAsync(user_role);
            Assert.IsTrue(deleted);
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
            var userService = TestUtils.CreateUserService();
            User user = ModelsInit.CreateUser();
            if (userService != null)
            {
                var dbUser = await userService.GetUserByUsernameAndPassword(user.Email, user.Password);
                Assert.IsNotNull(dbUser);
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




