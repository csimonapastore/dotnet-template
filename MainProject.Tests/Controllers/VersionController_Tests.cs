using System;
using System.Reflection;
using System.Net;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BasicDotnetTemplate.MainProject;
using BasicDotnetTemplate.MainProject.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using BasicDotnetTemplate.MainProject.Models.Api.Response;
using BasicDotnetTemplate.MainProject.Models.Settings;


namespace BasicDotnetTemplate.MainProject.Tests;

[TestClass]
public class BaseController_Tests
{
    [TestMethod]
    public void BaseController_NullConfiguration()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
        var exception = true;
        try
        {
            _ = new VersionController(null);
            exception = false;
            Assert.Fail($"This test should not pass as configuration is null");
        }
        catch (Exception)
        {
            Assert.IsTrue(exception);
        }
    }


    [TestMethod]
    public void VersionController_GetVersion_Valid()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");

        try
        {
            IConfiguration configuration = TestUtils.CreateConfiguration();
            VersionController versionController = new VersionController(configuration);
            var result = versionController.GetVersion();
            var objectResult = ((ObjectResult)result).Value;
            if (objectResult != null)
            {
                var data = (BaseResponse)objectResult;
                if (data.Data != null)
                {
                    AppSettings appSettings = new AppSettings();
                    configuration.GetSection("AppSettings").Bind(appSettings);
                    string version = data.Data != null ? (string)data.Data : "";
                    Assert.IsTrue((((IStatusCodeActionResult)result).StatusCode == 200) && (version == appSettings.Settings?.Version));
                }
                else
                {
                    Assert.Fail($"Unable to convert response value to BaseResponse because Data is null");
                }
            }
            else
            {
                Assert.Fail($"Data is null");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex.Message}");
        }
    }
}
