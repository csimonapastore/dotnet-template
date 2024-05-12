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


namespace BasicDotnetTemplate.MainProject.Tests;

[TestClass]
public class BaseController_Tests
{
    [TestMethod]
    public void BaseControllerInit_Valid()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");

        try
        {
            IConfiguration configuration = TestUtils.CreateConfiguration();
            VersionController versionController = new VersionController(configuration);
            var result = versionController.GetVersion();
            Assert.IsTrue(((IStatusCodeActionResult)result).StatusCode == 200);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex.Message}");
        }
    }
}
