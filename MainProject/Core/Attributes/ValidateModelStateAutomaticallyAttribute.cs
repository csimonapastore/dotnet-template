using System;

namespace BasicDotnetTemplate.MainProject.Core.Attributes
{
    /// <summary>
    /// Indicates that ModelState validation is handled automatically by an Action Filter.
    /// Used to suppress SonarCloud warnings about missing ModelState.IsValid checks.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ModelStateValidationHandledByFilterAttribute : Attribute
    { }
}
