using System;
using System.Reflection;
using System.Net;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BasicDotnetTemplate.MainProject;


namespace BasicDotnetTemplate.MainProject.Tests;

[TestClass]
public class Program_Tests
{
    [TestMethod]
    public async Task Program_Configuration_IsValid()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");

        try
        {
            var reflectionType = typeof(ReflectionProgram);

            if (reflectionType != null)
            {
                MethodInfo[] methods = reflectionType.GetMethods(); //Using BindingFlags.NonPublic does not show any results

                MethodInfo? execute = null;

                foreach (MethodInfo m in methods)
                {
                    if (m.Name == "LaunchConfiguration")
                    {
                        execute = m;
                    }
                }

                if (execute != null)
                {
                    object initializeObj = execute.Invoke(null, new object[] { });
                    MethodInfo initialize = (MethodInfo)initializeObj;
                    if (initialize != null)
                    {
                        initialize.Invoke(null, new object[] { new string[] { } });
                        Assert.IsTrue(true);
                    }
                    else
                    {
                        Assert.Fail("Initialize is null.");
                    }
                }
                else
                {
                    Assert.Fail("Initialize method not found in Program class.");
                }
            }
            else
            {
                Assert.Fail("Program class not found in the assembly.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            Assert.Fail($"An exception was thrown: {ex.Message}");
        }
    }
}
