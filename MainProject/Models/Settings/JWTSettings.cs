namespace BasicDotnetTemplate.MainProject.Models.Settings;

public class JWTSettings
{
#nullable enable
    public string? ValidAudience { get; set; }
    public string? ValidIssuer { get; set; }
    public string? Secret { get; set; }
    public int? ExpiredAfterMinsOfInactivity { get; set; }

#nullable disable
}