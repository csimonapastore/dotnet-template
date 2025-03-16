using System;
using System.Reflection;
using System.Net;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BasicDotnetTemplate.MainProject;
using BasicDotnetTemplate.MainProject.Models.Api.Response;
using Microsoft.Extensions.DependencyModel.Resolution;
using BasicDotnetTemplate.MainProject.Models.Settings;
using Microsoft.AspNetCore.Http;


namespace BasicDotnetTemplate.MainProject.Tests;

[TestClass]
public class Settings_Tests
{
    [TestMethod]
    public void IstantiatePrivateSettings_Valid()
    {
        try
        {
            var sqlServer = "Data Source=localhost; Initial Catalog=YourDatabase; User Id=YourUsername; Password=YourPassword;";
            var mongodb = "mongodb://localhost:27017";
            var postgres = "Server=localhost; Port=5432; Database=YourDatabase; User Id=YourUsername; Password=YourPassword;";

            var privateSettings = new PrivateSettings()
            {
                DatabaseConnection = new DatabaseConnection()
                {
                    SqlServer = sqlServer,
                    Mongodb = mongodb,
                    Postgres = postgres,
                }
            };

            Assert.IsTrue(
                privateSettings.DatabaseConnection != null &&
                privateSettings.DatabaseConnection.SqlServer == sqlServer &&
                privateSettings.DatabaseConnection.Mongodb == mongodb &&
                privateSettings.DatabaseConnection.Postgres == postgres
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public void IstantiateBaseResponse_OnlyStatus_IsInvalid()
    {
        try
        {
            var baseResponse = new BaseResponse<object>(201, null, null);
            Assert.IsFalse(baseResponse.Status == StatusCodes.Status200OK);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public void IstantiateBaseResponse_StatusAndMessage_Valid()
    {
        try
        {
            var baseResponse = new BaseResponse<object>(200, "This is a test message", null);
            Assert.IsTrue(baseResponse.Status == StatusCodes.Status200OK && baseResponse.Message == "This is a test message" && baseResponse.Data == null);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public void IstantiateBaseResponse_AllFields_Valid()
    {
        try
        {
            string[] data = { "Volvo", "BMW", "Ford", "Mazda" };
            var baseResponse = new BaseResponse<string[]>(200, "This is a test message", data);
            Assert.IsTrue(baseResponse.Status == StatusCodes.Status200OK && baseResponse.Message == "This is a test message" && baseResponse.Data == data);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }
}




