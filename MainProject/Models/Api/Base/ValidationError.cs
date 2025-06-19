using System;

namespace BasicDotnetTemplate.MainProject.Models.Api.Base;

public class ValidationError
{
    public string Message { get; set; }
    public Dictionary<string, List<string>> Errors { get; set; }
}
