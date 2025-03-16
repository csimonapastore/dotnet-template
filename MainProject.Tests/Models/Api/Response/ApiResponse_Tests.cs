using System;
using System.Reflection;
using System.Net;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BasicDotnetTemplate.MainProject;
using BasicDotnetTemplate.MainProject.Models.Api.Response;
using Microsoft.Extensions.DependencyModel.Resolution;
using Microsoft.AspNetCore.Http;


namespace BasicDotnetTemplate.MainProject.Tests;

[TestClass]
public class ApiResponse_Tests
{
    [TestMethod]
    public void IstantiateBaseResponse_OnlyStatus_Valid()
    {
        try
        {
            var baseResponse = new BaseResponse<object>(200, null, null);
            Assert.IsTrue(baseResponse.Status == StatusCodes.Status200OK && String.IsNullOrEmpty(baseResponse.Message) && baseResponse.Data == null);
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




