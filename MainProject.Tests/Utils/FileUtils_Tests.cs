using BasicDotnetTemplate.MainProject.Utils;
using BasicDotnetTemplate.MainProject.Models.Common;


namespace BasicDotnetTemplate.MainProject.Tests;

[TestClass]
public class FileUtils_Tests
{
    [TestMethod]
    public void ConvertFileToObject_NoFilePath()
    {
        try
        {
            PermissionsFile? permissionsFile = FileUtils.ConvertFileToObject<PermissionsFile>(String.Empty);
            Assert.Fail($"Expected exception instead of response: {permissionsFile}");
        }
        catch (ArgumentException argumentException)
        {
            Assert.IsInstanceOfType(argumentException, typeof(ArgumentException));
        }
        catch (Exception exception)
        {
            Assert.Fail($"An exception was thrown: {exception}");
        }
    }

    [TestMethod]
    public void ConvertFileToObject_NoFile()
    {
        try
        {
            PermissionsFile? permissionsFile = FileUtils.ConvertFileToObject<PermissionsFile>(System.AppDomain.CurrentDomain.BaseDirectory + "Config/no-permissions.json");
            Assert.Fail($"Expected exception instead of response: {permissionsFile}");
        }
        catch (FileNotFoundException fileNotFoundException)
        {
            Assert.IsInstanceOfType(fileNotFoundException, typeof(FileNotFoundException));
        }
        catch (Exception exception)
        {
            Assert.Fail($"An exception was thrown: {exception}");
        }
    }

    [TestMethod]
    public void ConvertFileToObject()
    {
        try
        {
            PermissionsFile? permissionsFile = FileUtils.ConvertFileToObject<PermissionsFile>(System.AppDomain.CurrentDomain.BaseDirectory + "Config/permissions.json");
            Assert.IsTrue(permissionsFile != null);
        }
        catch (Exception exception)
        {
            Assert.Fail($"An exception was thrown: {exception}");
        }
    }

    [TestMethod]
    public void ConvertFileToObject_InvalidOperationException()
    {
        try
        {
            PermissionsFile? permissionsFile = FileUtils.ConvertFileToObject<PermissionsFile>(System.AppDomain.CurrentDomain.BaseDirectory + "Config/invalid-permissions.json");
            Assert.Fail($"Expected exception instead of response: {permissionsFile}");
        }
        catch (InvalidOperationException invalidOperationException)
        {
            Assert.IsInstanceOfType(invalidOperationException, typeof(InvalidOperationException));
        }
        catch (Exception exception)
        {
            Assert.Fail($"An exception was thrown: {exception}");
        }
    }

}




