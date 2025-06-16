namespace BasicDotnetTemplate.MainProject.Models.Settings;

public class EncryptionSettings
{
#nullable enable
    public string? Secret { get; set; }
    public string? Salt { get; set; }
    public int? Iterations { get; set; }
#nullable disable
}