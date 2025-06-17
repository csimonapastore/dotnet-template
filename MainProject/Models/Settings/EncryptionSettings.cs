namespace BasicDotnetTemplate.MainProject.Models.Settings;

public class EncryptionSettings
{
#nullable enable
    public string? SaltKey { get; set; }
    public string? Salt { get; set; }
    public int? Iterations { get; set; }
#nullable disable
}