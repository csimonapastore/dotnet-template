namespace BasicDotnetTemplate.MainProject.Models.Settings;

public class JwtSettings
{
#nullable enable
    public string? ValidAudience { get; set; }
    public string? ValidIssuer { get; set; }
    public string? Secret { get; set; }
    public int? ExpiredAfterMinsOfInactivity { get; set; }

#nullable disable
}
