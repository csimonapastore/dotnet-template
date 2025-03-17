
using BasicDotnetTemplate.MainProject.Services;
using BasicDotnetTemplate.MainProject.Models.Api.Data.Role;
using BasicDotnetTemplate.MainProject.Models.Database.SqlServer;



namespace BasicDotnetTemplate.MainProject.Tests;

[TestClass]
public class RoleService_Tests
{
    private static Role? _expectedRole;
    private static Role? _role;
    private static RoleService? _roleService;

    [TestInitialize]
    public void Setup()
    {
        _expectedRole = ModelsInit.CreateRole();
        _roleService = TestUtils.CreateRoleService();
    }

    [TestMethod]
    public void Inizialize()
    {
        try
        {
            if (_roleService != null)
            {
                Assert.IsInstanceOfType(_roleService, typeof(RoleService));
            }
            else
            {
                Assert.Fail($"RoleService is null");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task CheckIfNameIsValid_NameNotExists()
    {
        try
        {
            if (_roleService != null)
            {
                var valid = await _roleService.CheckIfNameIsValid(_expectedRole?.Name ?? String.Empty);
                Assert.IsTrue(valid);
            }
            else
            {
                Assert.Fail($"RoleService is null");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task CreateRoleData()
    {
        try
        {
            CreateRoleRequestData data = new CreateRoleRequestData()
            {
                Name = _expectedRole?.Name ?? String.Empty,
                IsNotEditable = false
            };

            if (_roleService != null)
            {
                var role = await _roleService.CreateRoleAsync(data);
                Assert.IsInstanceOfType(role, typeof(Role));
                Assert.IsNotNull(role);
                Assert.IsTrue(_expectedRole?.Name == role.Name);
                Assert.IsTrue(_expectedRole?.IsNotEditable == role.IsNotEditable);
                _role = role;
            }
            else
            {
                Assert.Fail($"RoleService is null");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task CheckIfNameIsValid_NameCurrentRole()
    {
        try
        {
            if (_roleService != null)
            {
                var valid = await _roleService.CheckIfNameIsValid(_expectedRole?.Name ?? String.Empty, _role?.Guid ?? String.Empty);
                Assert.IsTrue(valid);
            }
            else
            {
                Assert.Fail($"RoleService is null");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task CheckIfNameIsValid_NameAlreadyExists()
    {
        try
        {
            if (_roleService != null)
            {
                var valid = await _roleService.CheckIfNameIsValid(_expectedRole?.Name ?? String.Empty);
                Assert.IsFalse(valid);
            }
            else
            {
                Assert.Fail($"RoleService is null");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task GetRoleByIdAsync()
    {
        try
        {
            if (_roleService != null)
            {
                var role = await _roleService.GetRoleByIdAsync(_role?.Id ?? 0);
                Assert.IsNotNull(role);
                Assert.IsTrue(role.Id == _role?.Id);
            }
            else
            {
                Assert.Fail($"RoleService is null");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task GetRoleByGuidAsync()
    {
        try
        {
            if (_roleService != null)
            {
                var role = await _roleService.GetRoleByGuidAsync(_role?.Guid ?? String.Empty);
                Assert.IsNotNull(role);
                Assert.IsTrue(role.Guid == _role?.Guid);
            }
            else
            {
                Assert.Fail($"RoleService is null");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public async Task DeleteRole()
    {
        try
        {
            if (_roleService != null)
            {
                var role = await _roleService.GetRoleByGuidAsync(_role?.Guid ?? String.Empty);
                Assert.IsNotNull(role);
                var deleted = await _roleService.DeleteRoleAsync(role);
                Assert.IsTrue(deleted);
            }
            else
            {
                Assert.Fail($"RoleService is null");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }


}




