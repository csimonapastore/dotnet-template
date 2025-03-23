using BasicDotnetTemplate.MainProject.Services;
using BasicDotnetTemplate.MainProject.Models.Api.Data.User;
using BasicDotnetTemplate.MainProject.Models.Database.SqlServer;

namespace BasicDotnetTemplate.MainProject.Tests;

[TestClass]
[TestCategory("Integration")]
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
    public async Task CreateUser_And_GetById_ReturnsCorrectUserAsync()
    {
        // Arrange
        var userService = TestUtils.CreateUserService();
        var user = ModelsInit.CreateUser();
        var role = ModelsInit.CreateRole();
        CreateUserRequestData data = new CreateUserRequestData()
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Password = user.Password
        };

        // Act 
        var createdUser = await userService.CreateUserAsync(data, role);
        Assert.IsNotNull(createdUser);

        // Act 
        var retrievedUser = await userService.GetUserByIdAsync(createdUser.Id);
        Assert.IsNotNull(retrievedUser);

        // Assert
        Assert.AreEqual(createdUser.Id, retrievedUser.Id);
        Assert.AreEqual(createdUser.Guid, retrievedUser.Guid);
    }
}




