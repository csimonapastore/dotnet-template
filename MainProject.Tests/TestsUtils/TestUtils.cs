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
using Microsoft.EntityFrameworkCore;
using Moq;
using BasicDotnetTemplate.MainProject.Core.Database;
using BasicDotnetTemplate.MainProject.Services;


namespace BasicDotnetTemplate.MainProject.Tests;

public static class TestUtils
{
    public static IConfiguration CreateConfiguration()
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(Array.Empty<string>());
        AppSettings appSettings = ProgramUtils.AddConfiguration(ref builder, System.AppDomain.CurrentDomain.BaseDirectory + "/JsonData");
        ProgramUtils.AddOpenApi(ref builder, appSettings);
        AppSettings _appSettings = new AppSettings();
        builder.Configuration.GetSection("AppSettings").Bind(_appSettings);
        return builder.Configuration;
    }

    public static IConfiguration CreateEmptyConfiguration(string? path = "", string? filename = "")
    {
        string appSettingsPath = String.IsNullOrEmpty(path) ? System.AppDomain.CurrentDomain.BaseDirectory : path;
        return new ConfigurationBuilder()
            .SetBasePath(appSettingsPath)
            .AddJsonFile(String.IsNullOrEmpty(filename) ? "appsettings.json" : filename, optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
    }

    public static AuthService CreateAuthService()
    {
        IConfiguration configuration = CreateConfiguration();
        var optionsBuilder = new DbContextOptionsBuilder<SqlServerContext>();
        optionsBuilder.UseSqlServer("test");
        SqlServerContext sqlServerContext = new SqlServerContext(optionsBuilder.Options);
        var userServiceMock = new Mock<IUserService>();
        return new AuthService(configuration, sqlServerContext, userServiceMock.Object);
    }

    public static UserService CreateUserService()
    {
        IConfiguration configuration = CreateConfiguration();
        var optionsBuilder = new DbContextOptionsBuilder<SqlServerContext>();
        optionsBuilder.UseSqlServer("test");
        SqlServerContext sqlServerContext = new SqlServerContext(optionsBuilder.Options);
        return new UserService(configuration, sqlServerContext);
    }

    public static JwtService CreateJwtService()
    {
        IConfiguration configuration = CreateConfiguration();
        var optionsBuilder = new DbContextOptionsBuilder<SqlServerContext>();
        optionsBuilder.UseSqlServer("test");
        SqlServerContext sqlServerContext = new SqlServerContext(optionsBuilder.Options);
        return new JwtService(configuration, sqlServerContext);
    }
}




