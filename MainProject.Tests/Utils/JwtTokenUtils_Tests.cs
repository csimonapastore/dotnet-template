using System;
using BasicDotnetTemplate.MainProject.Models.Settings;
using Microsoft.AspNetCore.Builder;
using BasicDotnetTemplate.MainProject.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;


namespace BasicDotnetTemplate.MainProject.Tests;

[TestClass]
public class JwtTokenUtils_Tests
{
    private static string _guid = "15e4be58-e655-475e-b4b8-a9779b359f57";

    [TestMethod]
    public void GenerateToken()
    {
        try
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(Array.Empty<string>());
            AppSettings appSettings = ProgramUtils.AddConfiguration(ref builder, System.AppDomain.CurrentDomain.BaseDirectory + "/JsonData");
            JwtTokenUtils jwtUtils = new JwtTokenUtils(appSettings);
            var jwt = jwtUtils.GenerateToken(_guid);
            Assert.IsTrue(!String.IsNullOrEmpty(jwt));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex.Message}");
        }
    }

    [TestMethod]
    public void ValidateToken()
    {
        try
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(Array.Empty<string>());
            AppSettings appSettings = ProgramUtils.AddConfiguration(ref builder, System.AppDomain.CurrentDomain.BaseDirectory + "/JsonData");
            JwtTokenUtils jwtUtils = new JwtTokenUtils(appSettings);
            var jwt = jwtUtils.GenerateToken(_guid);
            var guid = jwtUtils.ValidateToken($"Bearer {jwt}");
            Assert.IsTrue(_guid == guid);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex.Message}");
        }
    }

    [TestMethod]
    public void ValidateToken_Empty()
    {
        try
        {
             WebApplicationBuilder builder = WebApplication.CreateBuilder(Array.Empty<string>());
            AppSettings appSettings = ProgramUtils.AddConfiguration(ref builder, System.AppDomain.CurrentDomain.BaseDirectory + "/JsonData");
            JwtTokenUtils jwtUtils = new JwtTokenUtils(appSettings);
            var jwt = jwtUtils.GenerateToken(_guid);
            var guid = jwtUtils.ValidateToken(jwt);
            Assert.IsTrue(String.IsNullOrEmpty(guid));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex.Message}");
        }
    }

}




