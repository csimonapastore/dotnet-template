using BasicDotnetTemplate.MainProject.Utils;
using BasicDotnetTemplate.MainProject.Models.Common;
using BasicDotnetTemplate.MainProject.Enum;


namespace BasicDotnetTemplate.MainProject.Tests;

[TestClass]
public class PasswordUtils_Test
{
    [TestMethod]
    public void PasswordValidation_Valid()
    {
        try
        {
            List<string> errors = PasswordUtils.ValidatePassword("#aBcDeFgHi01245#");
            Assert.IsTrue(errors == null || errors.Count == 0);
        }
        catch (Exception exception)
        {
            Assert.Fail($"An exception was thrown: {exception}");
        }
    }

    [TestMethod]
    public void PasswordValidation_Invalid()
    {
        try
        {
            List<string> errors = PasswordUtils.ValidatePassword("aAa1#");
            Assert.IsTrue(errors.Contains(PasswordValidationEnum.MIN_LENGTH));
            Assert.IsTrue(errors.Contains(PasswordValidationEnum.MIN_UPPER));
            Assert.IsTrue(errors.Contains(PasswordValidationEnum.MIN_NUMBER));
            Assert.IsTrue(errors.Contains(PasswordValidationEnum.MIN_SPECIAL));
            Assert.IsTrue(errors.Contains(PasswordValidationEnum.IDENTICAL_CHARS));
            Assert.IsTrue(!errors.Contains(PasswordValidationEnum.MIN_LOWER));
        }
        catch (Exception exception)
        {
            Assert.Fail($"An exception was thrown: {exception}");
        }
    }

    [TestMethod]
    public void PasswordValidation_ToLowerInvalid()
    {
        try
        {
            List<string> errors = PasswordUtils.ValidatePassword("AaBC0*TGH1#");
            Assert.IsTrue(errors.Contains(PasswordValidationEnum.MIN_LOWER));
        }
        catch (Exception exception)
        {
            Assert.Fail($"An exception was thrown: {exception}");
        }
    }

}




