using BasicDotnetTemplate.MainProject.Models.Api.Common.User;
using DatabaseSqlServer = BasicDotnetTemplate.MainProject.Models.Database.SqlServer;
using Microsoft.AspNetCore.Builder;
using BasicDotnetTemplate.MainProject.Models.Settings;
using BasicDotnetTemplate.MainProject.Utils;
using Microsoft.AspNetCore.Mvc.Filters;
using BasicDotnetTemplate.MainProject.Core.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;


namespace BasicDotnetTemplate.MainProject.Tests;

[TestClass]
public class JwtAuthorizationAttribute_Tests
{
    private static AuthenticatedUser? _authenticatedUser = null;
    private static string? _token = null;
    private static JwtAuthorizationAttribute? _attribute;
    private static JwtTokenUtils? _jwtTokenUtils;

    [TestInitialize]
    public void Setup()
    {
        _attribute = new JwtAuthorizationAttribute();

        DatabaseSqlServer.User user = ModelsInit.CreateUser();
        _authenticatedUser = new AuthenticatedUser(user);

        WebApplicationBuilder builder = WebApplication.CreateBuilder(Array.Empty<string>());
        AppSettings appSettings = ProgramUtils.AddConfiguration(ref builder, System.AppDomain.CurrentDomain.BaseDirectory + "/JsonData");
        _jwtTokenUtils = new JwtTokenUtils(appSettings);
        _token = _jwtTokenUtils.GenerateToken(user.Guid);
    }

    private static AuthorizationFilterContext CreateAuthorizationContextWithAllowAnonymous()
    {
        var httpContext = new DefaultHttpContext();
        var routeData = new RouteData();
        var actionDescriptor = new ControllerActionDescriptor
        {
            EndpointMetadata = new List<object> { new AllowAnonymousAttribute() }
        };
        var actionContext = new ActionContext(httpContext, routeData, actionDescriptor);

        actionContext.ActionDescriptor.EndpointMetadata.Add(new AllowAnonymousAttribute());

        return new AuthorizationFilterContext(actionContext, []);
    }

    private static AuthorizationFilterContext CreateAuthorizationContext()
    {
        var httpContext = new DefaultHttpContext();
        var actionContext = new ActionContext(httpContext, new RouteData(), new ControllerActionDescriptor());
        return new AuthorizationFilterContext(actionContext, new List<IFilterMetadata>());
    }

    [TestMethod]
    public void OnAuthorization_AllowAnonymous_SkipsAuthorization()
    {
        try
        {
            var context = CreateAuthorizationContextWithAllowAnonymous();
            _attribute?.OnAuthorization(context);

            Assert.IsNull(context.Result);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex}");
        }
    }

    [TestMethod]
    public void OnAuthorization_NoAuthenticatedUser_ReturnsUnauthorized()
    {
        var context = CreateAuthorizationContext();
        IConfiguration configuration = TestUtils.CreateConfiguration();

        context.HttpContext.RequestServices = new ServiceCollection()
            .AddSingleton(configuration)
            .BuildServiceProvider();

        context.HttpContext.Items["User"] = null;
        _attribute?.OnAuthorization(context);
        Assert.IsInstanceOfType(context.Result, typeof(UnauthorizedResult));
    }

    [TestMethod]
    public void OnAuthorization_EmptyAuthorizationHeader_ReturnsUnauthorized()
    {
        var context = CreateAuthorizationContext();
        IConfiguration configuration = TestUtils.CreateConfiguration();

        context.HttpContext.RequestServices = new ServiceCollection()
            .AddSingleton(configuration)
            .BuildServiceProvider();

        context.HttpContext.Items["User"] = _authenticatedUser;
        context.HttpContext.Request.Headers.Authorization = string.Empty;

        _attribute?.OnAuthorization(context);

        Assert.IsInstanceOfType(context.Result, typeof(UnauthorizedResult));
    }


    [TestMethod]
    public void OnAuthorization_InvalidToken_ReturnsUnauthorized()
    {
        var context = CreateAuthorizationContext();
        IConfiguration configuration = TestUtils.CreateConfiguration();

        context.HttpContext.RequestServices = new ServiceCollection()
            .AddSingleton(configuration)
            .BuildServiceProvider();

        var invalidToken = _jwtTokenUtils?.GenerateToken(Guid.NewGuid().ToString());
        context.HttpContext.Request.Headers.Authorization = $"Bearer {invalidToken}";

        context.HttpContext.Items["User"] = _authenticatedUser;

        _attribute?.OnAuthorization(context);

        Assert.IsInstanceOfType(context.Result, typeof(UnauthorizedResult));
    }

    [TestMethod]
    public void OnAuthorization_ValidToken()
    {
        var context = CreateAuthorizationContext();
        IConfiguration configuration = TestUtils.CreateConfiguration();

        context.HttpContext.RequestServices = new ServiceCollection()
            .AddSingleton(configuration)
            .BuildServiceProvider();
        context.HttpContext.Request.Headers.Authorization = $"Bearer {_token}";

        context.HttpContext.Items["User"] = _authenticatedUser;

        _attribute?.OnAuthorization(context);

        Assert.IsNotInstanceOfType(context.Result, typeof(UnauthorizedResult));
    }

}