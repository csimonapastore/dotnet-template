using System;
using System.Reflection;
using System.Net;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BasicDotnetTemplate.MainProject;
using BasicDotnetTemplate.MainProject.Models.Api.Response;
using Microsoft.Extensions.DependencyModel.Resolution;
using BasicDotnetTemplate.MainProject.Models.Api.Common.User;
using BasicDotnetTemplate.MainProject.Models.Api.Response.User;
using DatabaseSqlServer = BasicDotnetTemplate.MainProject.Models.Database.SqlServer;
using BasicDotnetTemplate.MainProject.Models.Api.Response.Auth;
using BasicDotnetTemplate.MainProject.Core.Middlewares;
using AutoMapper;


namespace BasicDotnetTemplate.MainProject.Tests;

[TestClass]
public class AutoMapperConfiguration_Tests
{
    private IMapper? _mapper;

    [TestInitialize]
    public void Setup()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AutoMapperConfiguration>();
        });

        _mapper = config.CreateMapper();
    }

    [TestMethod]
    public void Mapper_UserDto()
    {
        try
        {
            DatabaseSqlServer.User user = ModelsInit.CreateUser();
            UserDto? data = _mapper?.Map<UserDto>(user);

            Assert.AreEqual(user.Guid, data?.Guid);
            Assert.AreEqual(user.FirstName, data?.FirstName);
            Assert.AreEqual(user.LastName, data?.LastName);
            Assert.AreEqual(user.Email, data?.Email);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

}




