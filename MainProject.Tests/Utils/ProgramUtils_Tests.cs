using System;
using System.Reflection;
using System.Net;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BasicDotnetTemplate.MainProject;
using BasicDotnetTemplate.MainProject.Models.Api.Response;
using Microsoft.Extensions.DependencyModel.Resolution;
using BasicDotnetTemplate.MainProject.Models.Settings;
using Microsoft.AspNetCore.Builder;
using BasicDotnetTemplate.MainProject.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Interfaces;


namespace BasicDotnetTemplate.MainProject.Tests;

[TestClass]
public class ProgramUtils_Tests
{
    [TestMethod]
    public void IstantiateAppSettingsOpenApi_NoOpenApiConfig_Valid()
    {
        try
        {
            OpenApiInfo expectedOpenApiInfo = new OpenApiInfo()
            {
                Title = "MainProject",
                Description = "This template contains basic configuration for a .Net 8 backend",
                Version = "v1.0",
                Contact = null,
                TermsOfService = null,
                License = null,
                Extensions = new Dictionary<string, IOpenApiExtension>()
            };

            WebApplicationBuilder builder = WebApplication.CreateBuilder(Array.Empty<string>());
            AppSettings realAppSettings = ProgramUtils.AddConfiguration(ref builder, "D:\\Users\\Simona\\Documents\\Projects\\BasicDotnetTemplate\\MainProject.Tests\\JsonData");
            OpenApiInfo realOpenApiInfo = ProgramUtils.CreateOpenApiInfo(realAppSettings);

            var areEquals = expectedOpenApiInfo.Title == realOpenApiInfo.Title &&
                expectedOpenApiInfo.Description == realOpenApiInfo.Description &&
                expectedOpenApiInfo.Version == realOpenApiInfo.Version &&
                expectedOpenApiInfo.Contact == realOpenApiInfo.Contact &&
                expectedOpenApiInfo.TermsOfService == realOpenApiInfo.TermsOfService &&
                expectedOpenApiInfo.License == realOpenApiInfo.License;

            Assert.IsTrue(areEquals);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex.Message}");
        }
    }

    [TestMethod]
    public void IstantiateAppSettingsOpenApi_OpenApiConfig_NotNull()
    {
        try
        {
            OpenApiInfo expectedOpenApiInfo = new OpenApiInfo()
            {
                Title = "MainProject",
                Description = "This template contains basic configuration for a .Net 8 backend",
                Version = "v1.0",
                Contact = null,
                TermsOfService = null,
                License = null,
                Extensions = new Dictionary<string, IOpenApiExtension>()
            };

            WebApplicationBuilder builder = WebApplication.CreateBuilder(Array.Empty<string>());
            AppSettings realAppSettings = ProgramUtils.AddConfiguration(ref builder, "D:\\Users\\Simona\\Documents\\Projects\\BasicDotnetTemplate\\MainProject.Tests\\JsonData", "completeAppSettings.json");
            OpenApiInfo realOpenApiInfo = ProgramUtils.CreateOpenApiInfo(realAppSettings);

            var areEquals = expectedOpenApiInfo.Title == realOpenApiInfo.Title &&
                expectedOpenApiInfo.Description == realOpenApiInfo.Description &&
                expectedOpenApiInfo.Version == realOpenApiInfo.Version &&
                expectedOpenApiInfo.Contact == realOpenApiInfo.Contact &&
                expectedOpenApiInfo.TermsOfService == realOpenApiInfo.TermsOfService &&
                expectedOpenApiInfo.License == realOpenApiInfo.License;

            Assert.IsFalse(areEquals);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex.Message}");
        }
    }


}




