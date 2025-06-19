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
using Microsoft.AspNetCore.Mvc.ModelBinding;
using BasicDotnetTemplate.MainProject.Core.Filters;
using Newtonsoft.Json;
using BasicDotnetTemplate.MainProject.Models.Api.Base;


namespace BasicDotnetTemplate.MainProject.Tests;

[TestClass]
public class ValidationActionFilter_Tests
{
    private readonly string _requestNotWellFormedMessage = "Request is not well formed";

    private static ActionExecutingContext CreateContext(ModelStateDictionary modelState, object? requestBody = null)
    {
        var actionContext = new ActionContext(
            new DefaultHttpContext(),
            new Microsoft.AspNetCore.Routing.RouteData(),
            new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor(),
            modelState
        );
        var actionArguments = new Dictionary<string, object?>();

        if (requestBody != null)
        {
            actionArguments.Add("request", requestBody);
        }
        return new ActionExecutingContext(
            actionContext,
            [],
            actionArguments,
            new Mock<Controller>().Object
        );
    }

    [TestMethod]
    public void OnActionExecutionAsync_ModelStateInvalid_ReturnsBadRequestAndDoesNotCallNext()
    {
        // Arrange
        var modelState = new ModelStateDictionary();
        modelState.AddModelError("MissingProperty", "MissingProperty is required");
        var context = CreateContext(modelState, new { SomeProp = "Value" });
        var nextCalled = false;
        ActionExecutionDelegate next = () => {
            nextCalled = true;
            return Task.FromResult(new ActionExecutedContext(context, [], new Mock<Controller>().Object));
        };
        var filter = new ValidationActionFilter();
        // Act
        filter.OnActionExecutionAsync(context, next).GetAwaiter().GetResult();
        // Assert
        Assert.IsNotNull(context.Result);
        var badRequestResult = context.Result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.IsNotNull(badRequestResult!.Value);

        ValidationError validationError = (ValidationError)badRequestResult.Value;
        Assert.AreEqual(_requestNotWellFormedMessage, validationError?.Message);
        Assert.IsNotNull(validationError?.Errors);
        Assert.IsFalse(modelState.IsValid);
        Assert.IsFalse(nextCalled);
    }

    [TestMethod]
    public void OnActionExecutionAsync_ModelStateValid_RequestBodyNull_ReturnsBadRequestAndDoesNotCallNext()
    {

        var modelState = new ModelStateDictionary();

        var context = CreateContext(modelState, null);
        var nextCalled = false;
        ActionExecutionDelegate next = () => {
            nextCalled = true;
            return Task.FromResult(new ActionExecutedContext(context, [], new Mock<Controller>().Object));
        };
        var filter = new ValidationActionFilter();
        // Act
        filter.OnActionExecutionAsync(context, next).GetAwaiter().GetResult();
        // Assert
        Assert.IsNotNull(context.Result);
        var badRequestResult = context.Result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.IsNotNull(badRequestResult!.Value);

        ValidationError validationError = (ValidationError)badRequestResult.Value;
        Console.WriteLine(JsonConvert.SerializeObject(validationError));
        Assert.AreEqual(_requestNotWellFormedMessage, validationError?.Message);
        Assert.IsNull(validationError?.Errors);
        Assert.IsTrue(modelState.IsValid);
        Assert.IsFalse(nextCalled);
    }


    [TestMethod]
    public void OnActionExecutionAsync_ModelStateValid_RequestBodyValid_CallsNextAndDoesNotSetResult()
    {
        // Arrange
        var modelState = new ModelStateDictionary();

        var requestBody = new TestRequestBody { Value = "Test" };
        var context = CreateContext(modelState, requestBody);
        var nextCalled = false;
        ActionExecutionDelegate next = () => {
            nextCalled = true;
            return Task.FromResult(new ActionExecutedContext(context, [], new Mock<Controller>().Object));
        };
        var filter = new ValidationActionFilter();
        // Act
        filter.OnActionExecutionAsync(context, next).GetAwaiter().GetResult();
        // Assert
        Assert.IsNull(context.Result);
        Assert.IsTrue(nextCalled);
    }


    private class TestRequestBody
    {
        public string? Value { get; set; }
    }

}
