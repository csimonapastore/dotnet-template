using BasicDotnetTemplate.MainProject.Utils;
using BasicDotnetTemplate.MainProject.Models.Common;


namespace BasicDotnetTemplate.MainProject.Tests;

[TestClass]
public static class FileUtils_Tests
{
    [TestMethod]
    public static void ConvertFileToObject_NoFilePath()
    {
        try
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
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public static void ConvertFileToObject_NoFile()
    {
        try
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
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

}




