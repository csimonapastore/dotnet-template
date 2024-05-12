using System;
using System.Reflection;
using System.Net;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BasicDotnetTemplate.MainProject;
using BasicDotnetTemplate.MainProject.Models.Api.Response;
using Microsoft.Extensions.DependencyModel.Resolution;


namespace BasicDotnetTemplate.MainProject.Tests;

[TestClass]
public class ApiResponse_Tests
{
    [TestMethod]
    public void IstantiateBaseResponse_OnlyStatus_Valid()
    {
        try
        {
            var baseResponse = new BaseResponse(200, null, null);
            Assert.IsTrue(baseResponse.Status == 200 && String.IsNullOrEmpty(baseResponse.Message) && baseResponse.Data == null);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex.Message}");
        }
    }

    [TestMethod]
    public void IstantiateBaseResponse_OnlyStatus_IsInvalid()
    {
        try
        {
            var baseResponse = new BaseResponse(201, null, null);
            Assert.IsFalse(baseResponse.Status == 200);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex.Message}");
        }
    }

    [TestMethod]
    public void IstantiateBaseResponse_StatusAndMessage_Valid()
    {
        try
        {
            var baseResponse = new BaseResponse(200, "This is a test message", null);
            Assert.IsTrue(baseResponse.Status == 200 && baseResponse.Message == "This is a test message" && baseResponse.Data == null);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex.Message}");
        }
    }

    [TestMethod]
    public void IstantiateBaseResponse_AllFields_Valid()
    {
        try
        {
            string[] data = { "Volvo", "BMW", "Ford", "Mazda" };
            var baseResponse = new BaseResponse(200, "This is a test message", data);
            Assert.IsTrue(baseResponse.Status == 200 && baseResponse.Message == "This is a test message" && baseResponse.Data == data);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex.Message}");
        }
    }
}




