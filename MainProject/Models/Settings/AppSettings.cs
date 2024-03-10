namespace BasicDotnetTemplate.MainProject.Models.Settings;

public class AppSettings
{
#nullable enable
    public Settings? Settings { get; set; }
    public PrivateSettings? PrivateSettings { get; set; }
    public OpenApiSettings? OpenApiSettings { get; set; }

#nullable disable
}