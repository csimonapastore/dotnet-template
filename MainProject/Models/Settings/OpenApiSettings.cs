namespace BasicDotnetTemplate.MainProject.Models.Settings;

public class OpenApiSettings
{
#nullable enable
    public string? TermsOfServiceUrl { get; set; }
    public OpenApiSettingsDetails? OpenApiContact { get; set; }
    public OpenApiSettingsDetails? OpenApiLicense { get; set; }
#nullable disable
}
